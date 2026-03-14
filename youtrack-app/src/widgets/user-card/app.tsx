import React, {memo} from 'react';
import ProgressBar from '@jetbrains/ring-ui-built/components/progress-bar/progress-bar';

import {type UserCardData} from './types';
import {mockUserCardData} from './mock-data';

await YTApp.register();

interface UserCardProps {
    data: UserCardData;
}

const UserCard: React.FunctionComponent<UserCardProps> = ({data}) => (
  <div className="widget">
    {/*<div className="stats-row">*/}
    {/*  <span className="stat">XP: {data.xp}</span>*/}
    {/*  <span className="stat">Balance: {data.balance}</span>*/}
    {/*</div>*/}
    <div className="level-row">
      <div className="level-header">
        <span className="level-label">Level {data.level}</span>
        <span className="level-xp">{data.xp} / {data.maxXp}</span>
      </div>
      <ProgressBar value={data.xp} max={data.maxXp}/>
    </div>
    <div className="achievements-row">
      <span className="stat">Balance: {data.balance}</span>
      {data.achievements.map(achievement => (
        <img
          key={achievement.id}
          className="achievement-icon"
          src={achievement.imageUrl}
          alt={achievement.description}
          title={achievement.description}
          width={24}
          height={24}
        />
            ))}
    </div>
  </div>
);

const AppComponent: React.FunctionComponent = () => (
  <UserCard data={mockUserCardData}/>
);

export const App = memo(AppComponent);
