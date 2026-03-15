import React, {memo, useEffect, useState} from 'react';
import Island from '@jetbrains/ring-ui-built/components/island/island';
import Header from '@jetbrains/ring-ui-built/components/island/header';
import Content from '@jetbrains/ring-ui-built/components/island/content';
import Button from '@jetbrains/ring-ui-built/components/button/button';
import Input from '@jetbrains/ring-ui-built/components/input/input';
import Select from '@jetbrains/ring-ui-built/components/select/select';
import Checkbox from '@jetbrains/ring-ui-built/components/checkbox/checkbox';

import {type HostAPI} from '../../../@types/globals';
import {type ProjectConfiguration} from './types';
import './app.css';

const host = await YTApp.register() as HostAPI;
const PROJECT_ID = 'SCR';

const WEIGHT_TYPE_OPTIONS = [
  {key: 'None', label: 'Не учитывать (Выкл)'},
  {key: 'StoryPoints', label: 'В Story Points'},
  {key: 'Time', label: 'В Часах (Time tracking)'},
];

const PRIORITY_COLORS: Record<string, string> = {
  Minor: '#888',
  Normal: '#2b90ce',
  Major: '#ff9900',
  Critical: '#E91E63',
};

const PRIORITY_BOLD: Record<string, boolean> = {
  Critical: true,
};

const ACHIEVEMENT_LABELS: Record<string, string> = {
  TaskBuilder: '🎯 Task Builder',
  OnFire: '🔥 On Fire',
  DeadlineHero: '🦸 Deadline Hero',
  BugHunter: '🐛 Bug Hunter',
  NightOwl: '🦉 Night Owl',
  Sheeva: '💪 Sheeva',
};

async function fetchConfig(): Promise<ProjectConfiguration | null> {
  try {
    return await host.fetchApp('backend/project-configuration', {
      query: {projectId: PROJECT_ID},
    }) as ProjectConfiguration;
  } catch {
    return null;
  }
}

async function saveConfig(config: ProjectConfiguration): Promise<boolean> {
  try {
    await host.fetchApp('backend/project-configuration', {
      method: 'PUT',
      body: config,
    });
    return true;
  } catch {
    return false;
  }
}

const AppComponent: React.FunctionComponent = () => {
  const [config, setConfig] = useState<ProjectConfiguration | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    (async () => {
      const data = await fetchConfig();
      setConfig(data);
      setLoading(false);
    })();
  }, []);

  if (loading) {
    return null;
  }

  if (!config) {
    return (
      <Island>
        <Content>
          <div className="access-denied">Ошибка загрузки конфигурации.</div>
        </Content>
      </Island>
    );
  }

  const handleSave = async () => {
    setSaving(true);
    const success = await saveConfig(config);
    setSaving(false);
    host.alert(
      success ? 'Конфигурация успешно сохранена!' : 'Ошибка при сохранении конфигурации.',
      undefined,
      3000
    );
  };

  const updateField = <K extends keyof ProjectConfiguration>(key: K, value: ProjectConfiguration[K]) => {
    setConfig({...config, [key]: value});
  };

  const updatePriority = (priority: string, value: number) => {
    setConfig({
      ...config,
      priorityMultipliers: {...config.priorityMultipliers, [priority]: value},
    });
  };

  const updateAchievementReward = (key: string, value: number) => {
    setConfig({
      ...config,
      achievementRewards: {...config.achievementRewards, [key]: value},
    });
  };

  const updateAchievementEnabled = (key: string, enabled: boolean) => {
    setConfig({
      ...config,
      achievementEnabled: {...config.achievementEnabled, [key]: enabled},
    });
  };

  const selectedWeightType = WEIGHT_TYPE_OPTIONS.find(o => o.key === config.issueWeightType) ?? WEIGHT_TYPE_OPTIONS[0];

  return (
    <Island style={{width: '100%', boxSizing: 'border-box'}}>
      <Header border style={{display: 'flex', alignItems: 'center', gap: '8px', fontSize: '14px', backgroundColor: '#F8F9FA'}}>
        <span style={{fontSize: '16px'}}>⚙️</span>
        <strong>GiveMeFive Config Panel (Admin)</strong>
      </Header>

      <Content>
        <div style={{display: 'flex', flexDirection: 'column', gap: '16px'}}>

          {/* Base XP */}
          <div>
            <h4 className="section-header">Базовый XP за тип задачи</h4>
            <div className="config-row">
              <span>За каждую закрытую <strong>Task</strong></span>
              <div className="input-group">
                <div className="input-wrap">
                  <Input
                    type="number"
                    value={config.issueResolveReward.toString()}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateField('issueResolveReward', Number(e.target.value) || 0)}
                  />
                </div>
                <span className="suffix">XP</span>
              </div>
            </div>
            <div className="config-row">
              <span>За каждый закрытый <strong>Bug</strong></span>
              <div className="input-group">
                <div className="input-wrap">
                  <Input
                    type="number"
                    value={config.bugResolveReward.toString()}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateField('bugResolveReward', Number(e.target.value) || 0)}
                  />
                </div>
                <span className="suffix">XP</span>
              </div>
            </div>
          </div>

          {/* Priority Multipliers */}
          <div>
            <h4 className="section-header">Множители Приоритетов</h4>
            {Object.entries(config.priorityMultipliers).map(([priority, value]) => (
              <div className="config-row" key={priority}>
                <span style={{
                  color: PRIORITY_COLORS[priority] ?? '#333',
                  fontWeight: PRIORITY_BOLD[priority] ? 'bold' : 'normal',
                }}>
                  {priority}
                </span>
                <div className="input-group">
                  <div className="input-wrap">
                    <Input
                      type="number"
                      step="0.1"
                      value={value.toString()}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => updatePriority(priority, Number(e.target.value) || 0)}
                    />
                  </div>
                  <span className="suffix"></span>
                </div>
              </div>
            ))}
          </div>

          {/* Complexity / Weight */}
          <div>
            <h4 className="section-header">Оценка сложности (Бонусный XP)</h4>
            <div className="config-row">
              <span>Считать сложность задачи:</span>
              <div style={{width: '200px'}}>
                <Select
                  data={WEIGHT_TYPE_OPTIONS}
                  selected={selectedWeightType}
                  onSelect={(item: {key: string} | null) => item && updateField('issueWeightType', item.key as ProjectConfiguration['issueWeightType'])}
                />
              </div>
            </div>
            {config.issueWeightType !== 'None' && (
              <>
                <div className="config-row">
                  <span>XP за 1 {config.issueWeightType === 'StoryPoints' ? 'Story Point' : 'Час'}:</span>
                  <div className="input-group">
                    <div className="input-wrap">
                      <Input
                        type="number"
                        value={config.issueUnitWeight.toString()}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateField('issueUnitWeight', Number(e.target.value) || 0)}
                      />
                    </div>
                    <span className="suffix">XP</span>
                  </div>
                </div>
                <div className="config-row">
                  <span>Название поля:</span>
                  <div className="input-wrap-narrow">
                    <Input
                      value={config.issueWeightFieldName}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateField('issueWeightFieldName', e.target.value)}
                    />
                  </div>
                </div>
              </>
            )}
          </div>

          {/* Achievement Rewards */}
          <div>
            <h4 className="section-header">Награды за Ачивки (Kanban)</h4>
            {Object.keys(config.achievementRewards).map(key => (
              <div className="config-row" key={key}>
                <div className="achievement-row">
                  <Checkbox
                    checked={config.achievementEnabled[key] ?? false}
                    onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateAchievementEnabled(key, e.target.checked)}
                  />
                  <span>{ACHIEVEMENT_LABELS[key] ?? key}</span>
                </div>
                <div className="input-group">
                  <div className="input-wrap">
                    <Input
                      type="number"
                      disabled={!config.achievementEnabled[key]}
                      value={config.achievementRewards[key].toString()}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => updateAchievementReward(key, Number(e.target.value) || 0)}
                    />
                  </div>
                  <span className="suffix">XP</span>
                </div>
              </div>
            ))}
          </div>

          {/* Save */}
          <div className="save-container">
            <Button primary disabled={saving} onClick={handleSave}>
              Сохранить изменения
            </Button>
          </div>

        </div>
      </Content>
    </Island>
  );
};

export const App = memo(AppComponent);
