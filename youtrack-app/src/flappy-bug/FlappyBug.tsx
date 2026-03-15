import React, { useState, useEffect, useCallback, useRef } from 'react';

const GAME_HEIGHT = 280;
const GRAVITY = 0.3; // Уменьшено на 25% (было 0.4)
const JUMP_STRENGTH = -5.5;

const SCROLL_SPEED = 1.8; // Чуть быстрее базовой, т.к. убрали рывок
const SPRINT_WIDTH = 120;
const SPRINT_GAP = 110; // Увеличено на ~20% (было 90)
const BUG_WIDTH = 32;
const BUG_HEIGHT = 24;
const BUG_LEFT = 60;

const BUG_PIXELS = [
  "       B B      ",
  "      B   B     ",
  "    BBBBBBBB    ",
  "   BGGGGGGGGBB  ",
  " BBLLBBGGBWWWWB ",
  "BLLLLBBGBWWPPPB ",
  "BLLLLBBGBWWPPPB ",
  " BBLLBBGGBWWWWB ",
  "   BBGGGGGBBBB  ",
  "     BBBBBB     ",
  "    B B  B B    ",
  "   B      B     "
];

const COLOR_MAP: Record<string, string> = {
  'B': '#212121', // Dark outline
  'G': '#8bc34a', // Bug Green
  'L': '#b3e5fc', // Wing Light Blue
  'W': '#ffffff', // Eye White
  'P': '#000000', // Pupil Black
};

const PixelBug: React.FC = () => {
  const pixelSize = BUG_HEIGHT / 12; // 2px
  return (
    <svg width={BUG_WIDTH} height={BUG_HEIGHT} shapeRendering="crispEdges">
      {BUG_PIXELS.map((row, y) => 
        row.split('').map((char, x) => {
          if (char === ' ') return null;
          return (
            <rect 
              key={`${x}-${y}`} 
              x={x * pixelSize} 
              y={y * pixelSize} 
              width={pixelSize} 
              height={pixelSize} 
              fill={COLOR_MAP[char]} 
            />
          )
        })
      )}
    </svg>
  );
};

// Fake task names and colors
const TASK_LABELS = ['Fix login', 'Sprint planning', 'Payment issue', 'Gantt redesign', 'UI glitch', 'Refactor API', 'Write docs', 'Deploy v2'];
const PREFIXES = ['BUG-', 'JT-', 'YT-'];
const TASK_COLORS = ['#e1f3d8', '#dcedff', '#fff0d4', '#ffdce0', '#f1e6ff'];
const BORDER_COLORS = ['#8bc34a', '#64b5f6', '#ffb74d', '#ef5350', '#ab47bc'];

interface TaskBar {
  id: string;
  label: string;
  color: string;
  borderColor: string;
  height: number;
}

interface SprintData {
  x: number;
  width: number;
  gapTop: number;
  topTasks: TaskBar[];
  bottomTasks: TaskBar[];
  passed: boolean; // to count score exactly once
}

export const FlappyBug: React.FC<{ 
  onClose: () => void;
  userId?: string;
  onScoreSubmit?: (score: number) => void;
}> = ({ onClose, userId = 'current_user_id', onScoreSubmit }) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const [scale, setScale] = useState(1);
  const [gameWidth, setGameWidth] = useState(550);
  const gameWidthRef = useRef(550);

  const [gameState, setGameState] = useState<'start' | 'playing' | 'gameover'>('start');
  const [birdPosition, setBirdPosition] = useState(GAME_HEIGHT / 2);
  const [score, setScore] = useState(0);
  const [sprints, setSprints] = useState<SprintData[]>([]);
  const [bgOffset, setBgOffset] = useState(0);
  
  const requestRef = useRef<number>();
  const birdVelocity = useRef(0);
  
  const generateTask = (): TaskBar => {
    const prefix = PREFIXES[Math.floor(Math.random() * PREFIXES.length)];
    const num = Math.floor(Math.random() * 900) + 10;
    const label = TASK_LABELS[Math.floor(Math.random() * TASK_LABELS.length)];
    const colorIdx = Math.floor(Math.random() * TASK_COLORS.length);
    return {
      id: `${prefix}${num}`,
      label,
      color: TASK_COLORS[colorIdx],
      borderColor: BORDER_COLORS[colorIdx],
      height: 20 + Math.floor(Math.random() * 8) // height 20-27
    };
  };

  const generateSprint = (xInit: number): SprintData => {
    const minHeight = 40;
    const maxHeight = GAME_HEIGHT - SPRINT_GAP - minHeight;
    const gapTop = Math.floor(Math.random() * (maxHeight - minHeight + 1)) + minHeight;
    
    // Generate tasks for top block
    let currentH = 0;
    const topTasks: TaskBar[] = [];
    while (currentH + 20 < gapTop) {
      const t = generateTask();
      const h = Math.min(t.height, gapTop - currentH);
      if (h > 10) {
        t.height = h;
        topTasks.push(t);
        currentH += h + 2; // small margin
      } else {
        break;
      }
    }

    // Generate tasks for bottom block
    const bottomHeight = GAME_HEIGHT - gapTop - SPRINT_GAP;
    currentH = 0;
    const bottomTasks: TaskBar[] = [];
    while (currentH + 20 < bottomHeight) {
      const t = generateTask();
      const h = Math.min(t.height, bottomHeight - currentH);
      if (h > 10) {
        t.height = h;
        bottomTasks.push(t);
        currentH += h + 2;
      } else {
        break;
      }
    }

    return {
      x: xInit,
      width: SPRINT_WIDTH,
      gapTop,
      topTasks,
      bottomTasks,
      passed: false
    };
  };

  const jump = useCallback(() => {
    if (gameState === 'start') {
      setGameState('playing');
      birdVelocity.current = JUMP_STRENGTH;
    } else if (gameState === 'playing') {
      birdVelocity.current = JUMP_STRENGTH;
    } else if (gameState === 'gameover') {
      setGameState('start');
      setBirdPosition(GAME_HEIGHT / 2);
      setSprints([]);
      setScore(0);
      setBgOffset(0);
      birdVelocity.current = 0;
    }
  }, [gameState]);

  const updateGame = useCallback(() => {
    if (gameState !== 'playing') return;

    setSprints((currentSprints) => {
      setBgOffset(o => o - SCROLL_SPEED);

      // Move sprints
      const newSprints = currentSprints.map(sp => ({ ...sp, x: sp.x - SCROLL_SPEED }));
      
      // Remove off-screen and add score
      if (newSprints.length > 0) {
        const first = newSprints[0];
        if (!first.passed && (first.x + first.width < BUG_LEFT)) {
          first.passed = true;
          setScore(s => s + 1);
        }
        if (first.x < -first.width) {
          newSprints.shift();
        }
      }
      
      // Add new sprint
      const currentSpawnX = gameWidthRef.current + 50;
      if (newSprints.length === 0 || newSprints[newSprints.length - 1].x < gameWidthRef.current - 250) {
        newSprints.push(generateSprint(currentSpawnX));
      }

      return newSprints;
    });

    setBirdPosition((pos) => {
      const newPos = pos + birdVelocity.current;
      birdVelocity.current += GRAVITY;
      
      // Floor collision
      if (newPos >= GAME_HEIGHT - BUG_HEIGHT + 4) {
        setGameState('gameover');
        return GAME_HEIGHT - BUG_HEIGHT + 4;
      }
      // Ceiling collision
      if (newPos <= -10) {
        setGameState('gameover');
        return -10;
      }
      return newPos;
    });

  }, [gameState]);

  // Collision detection
  useEffect(() => {
    if (gameState !== 'playing') return;
    
    // Hitbox
    const birdRect = {
      left: BUG_LEFT + 4,
      right: BUG_LEFT + BUG_WIDTH - 4,
      top: birdPosition + 4,
      bottom: birdPosition + BUG_HEIGHT - 4
    };

    for (const sp of sprints) {
      const spLeft = sp.x;
      const spRight = sp.x + sp.width;
      const topPipeBottom = sp.gapTop;
      const bottomPipeTop = sp.gapTop + SPRINT_GAP;

      // Check horizontal overlap
      if (birdRect.right > spLeft && birdRect.left < spRight) {
        // Check vertical overlap
        if (birdRect.top < topPipeBottom || birdRect.bottom > bottomPipeTop) {
          setGameState('gameover');
        }
      }
    }
  }, [birdPosition, sprints, gameState]);

  // Handle API Contract (Send Score on Game Over)
  useEffect(() => {
    if (gameState === 'gameover') {
      if (onScoreSubmit) {
        onScoreSubmit(score);
      }
      
      // Иллюстрация контракта для бэкенда
      console.log('--- MINIGAME API CONTRACT DEMO ---');
      console.log('POST /api/minigame/score');
      console.log('Payload:', JSON.stringify({ userId, score }));
      
      /* 
      // Пример реального вызова (раскомментировать при интеграции):
      fetch('/api/minigame/score', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ userId, score })
      }).catch(err => console.error('Error saving score:', err));
      */
    }
  }, [gameState, score, userId, onScoreSubmit]);

  useEffect(() => {
    if (gameState === 'playing') {
      requestRef.current = requestAnimationFrame(function loop() {
        updateGame();
        requestRef.current = requestAnimationFrame(loop);
      });
    }
    return () => {
      if (requestRef.current) cancelAnimationFrame(requestRef.current);
    };
  }, [gameState, updateGame]);

  useEffect(() => {
    if (!containerRef.current) return;
    const ro = new ResizeObserver(entries => {
      for (const entry of entries) {
        const { width, height } = entry.contentRect;
        // Scale to fit height perfectly
        const newScale = height / GAME_HEIGHT;
        // Calculate the logical width needed to fill exactly to the edges
        const newGameWidth = width / newScale;
        
        setScale(newScale);
        setGameWidth(newGameWidth);
        gameWidthRef.current = newGameWidth;
      }
    });
    ro.observe(containerRef.current);
    return () => ro.disconnect();
  }, []);

  return (
    <div 
      ref={containerRef}
      style={{ 
        width: '100%', 
        height: '100%', 
        backgroundColor: '#f8f9fa', 
        overflow: 'hidden',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        borderRadius: '8px',
        cursor: 'pointer',
        userSelect: 'none',
        fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif',
        border: '1px solid #e0e0e0',
        boxShadow: 'inset 0 2px 4px rgba(0,0,0,0.02)'
      }}
      onClick={jump}
    >
      {/* Scaled Inner Game Container */}
      <div style={{
        position: 'relative',
        width: gameWidth,
        height: GAME_HEIGHT,
        transform: `scale(${scale})`,
        transformOrigin: 'center center',
      }}>
        {/* Grid Background */}
        <div style={{
          position: 'absolute',
        top: 0, bottom: 0, left: 0, right: 0,
        backgroundImage: 'linear-gradient(to right, #edeff2 1px, transparent 1px)',
        backgroundSize: '40px 100%',
        backgroundPosition: `${bgOffset}px 0`,
        opacity: 0.6
      }} />

      {/* Tilted Bug */}
      <div style={{
        position: 'absolute',
        left: BUG_LEFT,
        top: birdPosition,
        width: BUG_WIDTH,
        height: BUG_HEIGHT,
        transition: gameState === 'playing' ? 'none' : 'top 0.3s ease',
        transform: `rotate(${Math.min(birdVelocity.current * 4, 90)}deg)`,
        zIndex: 10
      }}>
        <PixelBug />
      </div>

      {/* Sprints / Gantt Bars */}
      {sprints.map((sp, i) => (
        <React.Fragment key={i}>
          {/* Top Block */}
          <div style={{ position: 'absolute', left: sp.x, top: 0, width: sp.width, height: sp.gapTop, display: 'flex', flexDirection: 'column', padding: '4px', boxSizing: 'border-box' }}>
            {sp.topTasks.map((t, idx) => (
              <div key={idx} style={{
                height: t.height,
                backgroundColor: t.color,
                border: `1px solid ${t.borderColor}`,
                borderRadius: '4px',
                marginBottom: '2px',
                fontSize: '9px',
                color: '#333',
                padding: '2px 4px',
                overflow: 'hidden',
                whiteSpace: 'nowrap',
                textOverflow: 'ellipsis',
                fontWeight: 500,
                display: 'flex',
                alignItems: 'center',
                boxShadow: '0 1px 2px rgba(0,0,0,0.05)'
              }}>
                <span style={{ fontWeight: 'bold', marginRight: '4px', color: '#555' }}>{t.id}</span>
                {t.label}
              </div>
            ))}
          </div>

          {/* Bottom Block */}
          <div style={{ position: 'absolute', left: sp.x, top: sp.gapTop + SPRINT_GAP, width: sp.width, height: GAME_HEIGHT - (sp.gapTop + SPRINT_GAP), display: 'flex', flexDirection: 'column', padding: '4px', boxSizing: 'border-box' }}>
            {sp.bottomTasks.map((t, idx) => (
              <div key={idx} style={{
                height: t.height,
                backgroundColor: t.color,
                border: `1px solid ${t.borderColor}`,
                borderRadius: '4px',
                marginBottom: '2px',
                fontSize: '9px',
                color: '#333',
                padding: '2px 4px',
                overflow: 'hidden',
                whiteSpace: 'nowrap',
                textOverflow: 'ellipsis',
                fontWeight: 500,
                display: 'flex',
                alignItems: 'center',
                boxShadow: '0 1px 2px rgba(0,0,0,0.05)'
              }}>
                <span style={{ fontWeight: 'bold', marginRight: '4px', color: '#555' }}>{t.id}</span>
                {t.label}
              </div>
            ))}
          </div>
          
          {/* Sprint Group Outline (Optional to show it's a Sprint) */}
          <div style={{
            position: 'absolute', left: sp.x, top: 0, width: sp.width, height: GAME_HEIGHT,
            borderLeft: '1px dashed #cfd4d9', borderRight: '1px dashed #cfd4d9',
            pointerEvents: 'none', opacity: 0.5
          }} />
        </React.Fragment>
      ))}

      {/* UI Overlay - Start */}
      {gameState === 'start' && (
        <div style={{
          position: 'absolute', inset: 0, display: 'flex', flexDirection: 'column',
          alignItems: 'center', justifyContent: 'center', backgroundColor: 'rgba(255,255,255,0.85)', color: '#333',
          zIndex: 20
        }}>
          <div style={{ fontSize: '24px', fontWeight: 'bold', marginBottom: '8px', color: '#1a1a1a' }}>Unfixed Bug</div>
          <div style={{ fontSize: '14px', marginBottom: '4px', color: '#555' }}>Navigate through the Gantt chart</div>
          <div style={{ fontSize: '12px', opacity: 0.7 }}>Click to start</div>
        </div>
      )}

      {/* UI Overlay - Game Over */}
      {gameState === 'gameover' && (
        <div style={{
          position: 'absolute', inset: 0, display: 'flex', flexDirection: 'column',
          alignItems: 'center', justifyContent: 'center', backgroundColor: 'rgba(255,255,255,0.9)', color: '#333',
          zIndex: 20
        }}>
          <div style={{ fontSize: '22px', fontWeight: 'bold', color: score > 3 ? '#2e7d32' : '#d32f2f' }}>
            {score > 0 ? `Finally fixed after ${score} sprints` : 'Fixed immediately!'}
          </div>
          <div style={{ fontSize: '14px', margin: '12px 0', color: '#555' }}>
            Bug survived: <strong>{score}</strong> {score === 1 ? 'sprint' : 'sprints'}
          </div>
          <div style={{ 
            fontSize: '13px', 
            padding: '8px 16px', 
            backgroundColor: '#1976d2', 
            color: 'white', 
            borderRadius: '4px',
            boxShadow: '0 2px 4px rgba(0,0,0,0.1)'
          }}>
            Click to Restart
          </div>
        </div>
      )}

      {/* HUD Score */}
      {gameState === 'playing' && (
        <div style={{
          position: 'absolute', top: 12, right: 12, padding: '4px 8px',
          backgroundColor: 'white', border: '1px solid #ddd', borderRadius: '4px',
          fontSize: '13px', fontWeight: 'bold', color: '#444',
          boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
          zIndex: 20
        }}>
          Unfixed for: {score} {score === 1 ? 'sprint' : 'sprints'}
        </div>
      )}

      {/* Close button */}
      <div 
        onClick={(e) => { e.stopPropagation(); onClose(); }}
        style={{
          position: 'absolute', top: 12, left: 12, padding: '4px 8px',
          backgroundColor: 'white', border: '1px solid #ddd', borderRadius: '4px',
          color: '#555', fontSize: '12px', cursor: 'pointer', fontWeight: 'bold',
          transition: 'all 0.2s', boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
          zIndex: 30
        }}
        onMouseEnter={(e) => {
          e.currentTarget.style.backgroundColor = '#f0f0f0';
          e.currentTarget.style.color = '#333';
        }}
        onMouseLeave={(e) => {
          e.currentTarget.style.backgroundColor = 'white';
          e.currentTarget.style.color = '#555';
        }}
      >
        ✕ Close
      </div>
      
      </div> {/* End of inner scalable game wrapper */}
    </div>
  );
};
