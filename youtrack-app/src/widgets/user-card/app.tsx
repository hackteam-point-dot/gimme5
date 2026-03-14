import React, {memo} from 'react';

import {type UserCardData} from './types';
import {mockUserCardData} from './mock-data';

await YTApp.register();

interface UserCardProps {
  data: UserCardData;
}

const UserCard: React.FunctionComponent<UserCardProps> = ({data}) => (
  <div className="widget">
    <div className="stats-row">
      <span className="stat">XP: {data.xp}</span>
      <span className="stat">Balance: {data.balance}</span>
    </div>
    <div className="achievements-row">
      {data.achievements.map(achievement => (
        <img
          key={achievement.id}
          className="achievement-icon"
          src={achievement.imageUrl}
          alt={achievement.description}
          title={achievement.description}
          width={16}
          height={16}
        />
      ))}
    </div>
  </div>
);

const AppComponent: React.FunctionComponent = () => (
  <UserCard data={mockUserCardData}/>
);

export const App = memo(AppComponent);
