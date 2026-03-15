import React, { useState } from 'react';
import Island from '@jetbrains/ring-ui-built/components/island/island';
import Header from '@jetbrains/ring-ui-built/components/island/header';
import Content from '@jetbrains/ring-ui-built/components/island/content';
import Button from '@jetbrains/ring-ui-built/components/button/button';
import Input from '@jetbrains/ring-ui-built/components/input/input';
import Select from '@jetbrains/ring-ui-built/components/select/select';
import Checkbox from '@jetbrains/ring-ui-built/components/checkbox/checkbox';
import '@jetbrains/ring-ui-built/components/style.css';

export const AdminPanel: React.FC = () => {
  const [config, setConfig] = useState({
    baseXpTask: 50,
    baseXpBug: 70,
    multMinor: 1,
    multNormal: 1.5,
    multMajor: 2,
    multCritical: 3,
    achTaskBuilder: 50,
    achTaskBuilderEnabled: true,
    achOnFire: 200,
    achOnFireEnabled: true,
    achDeadlineHero: 200,
    achDeadlineHeroEnabled: true,
    achBugHunter: 100,
    achBugHunterEnabled: true,
    achNightOwl: 20,
    achNightOwlEnabled: true,
    leaderboardResetType: 'sprints',
    leaderboardResetCount: 1,
    complexityType: 'none',
    complexityMultiplier: 10,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const value = e.target.type === 'number' ? Number(e.target.value) || 0 : e.target.value;
    setConfig({
      ...config,
      [e.target.name]: value
    });
  };

  const rightContainerStyle = {
    display: 'flex',
    alignItems: 'center',
    gap: '8px',
  };

  const inputStyle = {
    width: '120px',
  };

  const suffixStyle = {
    width: '24px',
    textAlign: 'left' as const,
  };

  const rowStyle = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '8px',
    fontSize: '13px',
    paddingRight: '32px'
  };

  return (
    <Island style={{ width: '100%', boxSizing: 'border-box', marginTop: '20px' }}>
      <Header border style={{ display: 'flex', alignItems: 'center', gap: '8px', fontSize: '14px', backgroundColor: '#F8F9FA' }}>
        <span style={{ fontSize: '16px' }}>⚙️</span>
        <strong>GiveMeFive Config Panel (Admin)</strong>
      </Header>
      
      <Content>
        <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          
          {/* Section 1: Base XP */}
          <div>
            <h4 style={{ fontSize: '14px', margin: '0 0 8px 0', borderBottom: '1px solid #eee', paddingBottom: '4px', color: '#333' }}>Базовый XP за тип задачи</h4>
            <div style={rowStyle}>
              <span>За каждую закрытую <strong>Task</strong></span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="baseXpTask" value={config.baseXpTask.toString()} onChange={handleChange} type="number" /></div>
                <span style={suffixStyle}>XP</span>
              </div>
            </div>
            <div style={rowStyle}>
              <span>За каждый закрытый <strong>Bug</strong></span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="baseXpBug" value={config.baseXpBug.toString()} onChange={handleChange} type="number" /></div>
                <span style={suffixStyle}>XP</span>
              </div>
            </div>
          </div>

          {/* Section 2: Multipliers */}
          <div>
            <h4 style={{ fontSize: '14px', margin: '0 0 8px 0', borderBottom: '1px solid #eee', paddingBottom: '4px', color: '#333' }}>Множители Приоритетов</h4>
            <div style={rowStyle}>
              <span style={{ color: '#888' }}>Minor</span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="multMinor" value={config.multMinor.toString()} onChange={handleChange} type="number" step="0.1" /></div>
                <span style={suffixStyle}></span>
              </div>
            </div>
            <div style={rowStyle}>
              <span style={{ color: '#2b90ce' }}>Normal</span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="multNormal" value={config.multNormal.toString()} onChange={handleChange} type="number" step="0.1" /></div>
                <span style={suffixStyle}></span>
              </div>
            </div>
            <div style={rowStyle}>
              <span style={{ color: '#ff9900' }}>Major</span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="multMajor" value={config.multMajor.toString()} onChange={handleChange} type="number" step="0.1" /></div>
                <span style={suffixStyle}></span>
              </div>
            </div>
            <div style={rowStyle}>
              <span style={{ color: '#E91E63', fontWeight: 'bold' }}>Critical</span>
              <div style={rightContainerStyle}>
                <div style={inputStyle}><Input name="multCritical" value={config.multCritical.toString()} onChange={handleChange} type="number" step="0.1" /></div>
                <span style={suffixStyle}></span>
              </div>
            </div>
          </div>

          {/* Section 3: Complexity */}
          <div>
            <h4 style={{ fontSize: '14px', margin: '0 0 8px 0', borderBottom: '1px solid #eee', paddingBottom: '4px', color: '#333' }}>Оценка сложности (Бонусный XP)</h4>
            <div style={rowStyle}>
              <span>Считать сложность задачи:</span>
              <div style={{ width: '152px' }}>
                <Select 
                  data={[
                    { key: 'none', label: 'Не учитывать (Выкл)' },
                    { key: 'story_points', label: 'В Story Points' },
                    { key: 'time', label: 'В Часах (Time tracking)' }
                  ]}
                  selected={{ key: config.complexityType, label: config.complexityType === 'story_points' ? 'В Story Points' : config.complexityType === 'time' ? 'В Часах (Time tracking)' : 'Не учитывать (Выкл)' }}
                  onSelect={(item: any) => setConfig({ ...config, complexityType: item.key })}
                />
              </div>
            </div>
            {config.complexityType !== 'none' && (
              <div style={rowStyle}>
                <span>XP за 1 {config.complexityType === 'story_points' ? 'Story Point' : 'Час'}:</span>
                <div style={rightContainerStyle}>
                  <div style={inputStyle}>
                    <Input 
                      type="number" 
                      name="complexityMultiplier" 
                      value={config.complexityMultiplier.toString()} 
                      onChange={handleChange} 
                      min={1}
                    />
                  </div>
                  <span style={suffixStyle}>XP</span>
                </div>
              </div>
            )}
          </div>

          {/* Section 4: Achievements */}
          <div>
            <h4 style={{ fontSize: '14px', margin: '0 0 8px 0', borderBottom: '1px solid #eee', paddingBottom: '4px', color: '#333' }}>Награды за Ачивки (Kanban)</h4>
            <div style={rowStyle}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Checkbox checked={config.achTaskBuilderEnabled} onChange={(e: any) => setConfig({ ...config, achTaskBuilderEnabled: e.target.checked })} />
                <span>🎯 Task Builder (+1)</span>
              </div>
              <div style={rightContainerStyle}><div style={inputStyle}><Input disabled={!config.achTaskBuilderEnabled} name="achTaskBuilder" value={config.achTaskBuilder.toString()} onChange={handleChange} type="number" /></div> <span style={suffixStyle}>XP</span></div>
            </div>
            <div style={rowStyle}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Checkbox checked={config.achOnFireEnabled} onChange={(e: any) => setConfig({ ...config, achOnFireEnabled: e.target.checked })} />
                <span>🔥 On Fire</span>
              </div>
              <div style={rightContainerStyle}><div style={inputStyle}><Input disabled={!config.achOnFireEnabled} name="achOnFire" value={config.achOnFire.toString()} onChange={handleChange} type="number" /></div> <span style={suffixStyle}>XP</span></div>
            </div>
            <div style={rowStyle}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Checkbox checked={config.achDeadlineHeroEnabled} onChange={(e: any) => setConfig({ ...config, achDeadlineHeroEnabled: e.target.checked })} />
                <span>🦸‍♂️ Deadline Hero</span>
              </div>
              <div style={rightContainerStyle}><div style={inputStyle}><Input disabled={!config.achDeadlineHeroEnabled} name="achDeadlineHero" value={config.achDeadlineHero.toString()} onChange={handleChange} type="number" /></div> <span style={suffixStyle}>XP</span></div>
            </div>
            <div style={rowStyle}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Checkbox checked={config.achBugHunterEnabled} onChange={(e: any) => setConfig({ ...config, achBugHunterEnabled: e.target.checked })} />
                <span>🐛 Bug Hunter</span>
              </div>
              <div style={rightContainerStyle}><div style={inputStyle}><Input disabled={!config.achBugHunterEnabled} name="achBugHunter" value={config.achBugHunter.toString()} onChange={handleChange} type="number" /></div> <span style={suffixStyle}>XP</span></div>
            </div>
            <div style={rowStyle}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Checkbox checked={config.achNightOwlEnabled} onChange={(e: any) => setConfig({ ...config, achNightOwlEnabled: e.target.checked })} />
                <span>🦉 Night Owl</span>
              </div>
              <div style={rightContainerStyle}><div style={inputStyle}><Input disabled={!config.achNightOwlEnabled} name="achNightOwl" value={config.achNightOwl.toString()} onChange={handleChange} type="number" /></div> <span style={suffixStyle}>XP</span></div>
            </div>
          </div>

          {/* Section 5: Leaderboard Settings */}
          <div>
            <h4 style={{ fontSize: '14px', margin: '0 0 8px 0', borderBottom: '1px solid #eee', paddingBottom: '4px', color: '#333' }}>Настройки Лидерборда</h4>
            <div style={rowStyle}>
              <span>Сбрасывать Лидерборд каждые:</span>
              <div style={rightContainerStyle}>
                <div style={{ width: '60px' }}>
                  <Input 
                    type="number" 
                    name="leaderboardResetCount" 
                    value={config.leaderboardResetCount.toString()} 
                    onChange={handleChange} 
                    min={1}
                  />
                </div>
                <div style={{ width: '84px' }}>
                  <Select 
                    data={[
                      { key: 'sprints', label: 'Спринт(ов)' },
                      { key: 'days', label: 'Дней' }
                    ]}
                    selected={{ key: config.leaderboardResetType, label: config.leaderboardResetType === 'sprints' ? 'Спринт(ов)' : 'Дней' }}
                    onSelect={(item: any) => setConfig({ ...config, leaderboardResetType: item.key })}
                  />
                </div>
              </div>
            </div>
          </div>

          <div style={{ marginTop: '8px', display: 'flex', justifyContent: 'flex-end', paddingRight: '32px' }}>
            <Button primary onClick={() => alert('Конфигурация успешно сохранена в базу (Mock)!')}>
              Сохранить изменения
            </Button>
          </div>

        </div>
      </Content>
    </Island>
  );
};
