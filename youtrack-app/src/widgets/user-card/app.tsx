import React, {memo, useEffect, useState} from 'react';
import ProgressBar from '@jetbrains/ring-ui-built/components/progress-bar/progress-bar';
import {type UserCardData} from './types';
import {API_BASE_URL} from './config';

await YTApp.register();

async function fetchUserCardData(): Promise<UserCardData | null> {
    try {
        const userId = YTApp.entity?.id;
        if (!userId) {
            return null;
        }
        console.log(YTApp.entity);
        console.log(`${API_BASE_URL}/api/UserProfile/card?userId=${encodeURIComponent(userId)}`);
        const response = await fetch(`${API_BASE_URL}/api/UserProfile/card?userId=${encodeURIComponent(userId)}`);
        if (!response.ok) {
            return null;
        }
        return await response.json() as UserCardData;
    } catch {
        return null;
    }
}

interface UserCardProps {
    data: UserCardData;
}

const UserCard: React.FunctionComponent<UserCardProps> = ({data}) => (
  <div className="widget">
    <div className="level-row">
      <div className="level-header">
        <span className="level-label">Level {data.level}</span>
        <span className="level-xp">{data.xp} / {data.maxXp}</span>
      </div>
      <ProgressBar value={data.xp} max={data.maxXp}/>
    </div>
    <div className="achievements-row">
      <span className="user-balance">Balance: {data.balance}</span>
      <div className="achievements">
        {data.achievements.map(achievement => (
          <div key={achievement.id} className="achievement-item" title={achievement.description}>
            <img
              className="achievement-icon"
              src={achievement.imageUrl}
              alt={achievement.description}
              width={32}
              height={32}
            />
            {achievement.count > 1 && (
            <span className="achievement-count">{achievement.count}</span>
                        )}
          </div>
                ))}
      </div>
    </div>
  </div>
);

const AppComponent: React.FunctionComponent = () => {
    const [data, setData] = useState<UserCardData | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchUserCardData().then(result => {
            setData(result);
            setLoading(false);
        });
    }, []);

    if (loading) {
        return null;
    }

    if (!data) {
        return <div>The user is not participating in GiveMeFive challenges yet</div>;
    }

    return <UserCard data={data}/>;
};

export const App = memo(AppComponent);
