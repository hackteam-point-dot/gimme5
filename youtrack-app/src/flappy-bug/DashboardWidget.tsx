import React, { useState } from 'react';
import Island from '@jetbrains/ring-ui-built/components/island/island';
import Header from '@jetbrains/ring-ui-built/components/island/header';
import Content from '@jetbrains/ring-ui-built/components/island/content';
import ProgressBar from '@jetbrains/ring-ui-built/components/progress-bar/progress-bar';
import Tooltip from '@jetbrains/ring-ui-built/components/tooltip/tooltip';

import '@jetbrains/ring-ui-built/components/style.css';
import { FlappyBug } from './FlappyBug';

const ALL_ACHIEVEMENTS = [
  { id: 1, title: 'Task Builder', condition: 'Move any task to Done', icon: '/achievements/task-builder.png?v=2' },
  { id: 2, title: 'Deadline Hero', condition: 'Deliver a task exactly before its Due Date', icon: '/achievements/deadline-hero.png?v=2' },
  { id: 3, title: 'On Fire', condition: 'Close 5 or more tasks in a single day', icon: '/achievements/on-fire.png?v=2' },
  { id: 4, title: 'Bug Hunter', condition: 'Squash 5 bugs in total', icon: '/achievements/bug-hunter.png?v=2' },
  { id: 5, title: 'Night Owl', condition: 'Close a task during late night hours', icon: '/achievements/night-owl.png?v=2' },
];

const mockProfile = {
  level: 3,
  xp: 1450,
  nextLevelXp: 2200,
  // Пользователь получил только 2 ачивки
  earnedAchievements: [
    { id: 1, count: 12 },
    { id: 4, count: 2 },
  ]
};

export const DashboardWidget: React.FC = () => {
  const [clickCount, setClickCount] = useState(0);
  const [showGame, setShowGame] = useState(false);

  const handleBugAchievementClick = () => {
    setClickCount(prev => prev + 1);
    if (clickCount + 1 >= 5) {
      setShowGame(true);
      setClickCount(0);
    }
  };

  const progressRatio = mockProfile.xp / mockProfile.nextLevelXp;

  return (
    <Island style={{ width: '100%', boxSizing: 'border-box' }}>
      <Header border style={{ display: 'flex', alignItems: 'center', gap: '8px', fontSize: '13px' }}>
        <span style={{ fontSize: '16px' }}>👾</span>
        <strong>GiveMeFive user card</strong>
      </Header>
      <Content>
        {showGame ? (
          <FlappyBug onClose={() => setShowGame(false)} />
        ) : (
          <React.Fragment>
            {/* Progress Bar Section */}
            <div style={{ marginBottom: 12 }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-end', fontSize: '12px', marginBottom: '4px' }}>
                <span>Уровень <strong style={{ fontSize: '13px' }}>{mockProfile.level}</strong></span>
                <span><strong>{mockProfile.xp}</strong> <span style={{ color: '#737577' }}>/ {mockProfile.nextLevelXp} XP</span></span>
              </div>
              <ProgressBar value={progressRatio} style={{ width: '100%' }} />
            </div>

            {/* Horizontal scroll container for achievements */}
            <div style={{
              display: 'flex',
              overflowX: 'auto',
              gap: '12px',
              paddingBottom: '8px',
              // hides scrollbar for cleaner look
              scrollbarWidth: 'none',
              msOverflowStyle: 'none',
            }}>
              {[...ALL_ACHIEVEMENTS]
                .sort((a, b) => {
                  const countA = mockProfile.earnedAchievements.find(ea => ea.id === a.id)?.count || 0;
                  const countB = mockProfile.earnedAchievements.find(ea => ea.id === b.id)?.count || 0;
                  const isLockedA = countA === 0;
                  const isLockedB = countB === 0;

                  // Earned ones come first
                  if (!isLockedA && isLockedB) return -1;
                  // Locked ones go slightly later
                  if (isLockedA && !isLockedB) return 1;
                  // Keep original order otherwise
                  return a.id - b.id;
                })
                .map(ach => {
                const earned = mockProfile.earnedAchievements.find(ea => ea.id === ach.id);
                const count = earned ? earned.count : 0;
                const isLocked = count === 0;

                const tooltipText = isLocked 
                  ? `🔒 ${ach.title} — ${ach.condition}`
                  : `${ach.title} (x${count})`;

                return (
                  <Tooltip title={tooltipText} key={ach.id}>
                    <div style={{
                      position: 'relative',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      width: '44px',
                      height: '44px',
                      borderRadius: '8px',
                      border: '1px solid transparent', // Changed to transparent to let pixel art shine securely
                      cursor: (isLocked && ach.id !== 4) ? 'help' : 'pointer',
                      transition: 'transform 0.1s ease-in-out',
                      filter: isLocked ? 'grayscale(100%) opacity(40%)' : 'none',
                      userSelect: ach.id === 4 ? 'none' : 'auto'
                    }}
                    onMouseEnter={(e) => { if (!isLocked) e.currentTarget.style.transform = 'scale(1.1)'; }}
                    onMouseLeave={(e) => { if (!isLocked) e.currentTarget.style.transform = 'scale(1)'; }}
                    onClick={() => {
                      if (ach.id === 4) {
                        handleBugAchievementClick();
                      }
                    }}
                    >
                      <img src={ach.icon} alt={ach.title} style={{ width: '40px', height: '40px', objectFit: 'contain' }} />
                      {!isLocked && count > 1 && (
                        <div style={{
                          position: 'absolute',
                          top: '-6px',
                          right: '-6px',
                          backgroundColor: '#E91E63',
                          color: 'white',
                          fontSize: '11px',
                          fontWeight: 'bold',
                          padding: '1px 5px',
                          borderRadius: '10px',
                          lineHeight: '1.2',
                          border: '2px solid #FAFAFA'
                        }}>
                          x{count}
                        </div>
                      )}
                    </div>
                  </Tooltip>
                );
              })}
            </div>
          </React.Fragment>
        )}
      </Content>
    </Island>
  );
};
