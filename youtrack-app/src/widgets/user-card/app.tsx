import React, {memo} from 'react';

import {type Achievement} from './types';
import {mockAchievements} from './mock-data';

await YTApp.register();

interface AchievementRowProps {
  achievement: Achievement;
}

const AchievementRow: React.FunctionComponent<AchievementRowProps> = ({achievement}) => (
  <div className="achievement-row">
    <img
      className="achievement-icon"
      src={achievement.imageUrl}
      alt=""
      width={48}
      height={48}
    />
    <span className="achievement-text">{achievement.description}</span>
  </div>
);

const AppComponent: React.FunctionComponent = () => (
  <div className="widget">
    {mockAchievements.map(achievement => (
      <AchievementRow key={achievement.id} achievement={achievement}/>
    ))}
  </div>
);

export const App = memo(AppComponent);
