import React, {memo, useEffect, useState} from 'react';
import ProgressBar from '@jetbrains/ring-ui-built/components/progress-bar/progress-bar';
import {type Achievement, type UserCardData} from './types';
import {FlappyBug} from '../../flappy-bug/FlappyBug';

const host = await YTApp.register();

async function fetchUserCardData(): Promise<UserCardData | null> {
    try {
        const userLogin = YTApp.entity?.login;
        if (!userLogin) {
            return null;
        }

        return await host.fetchApp('backend/user-card', {query: {userId: userLogin}}) as UserCardData;
    } catch {
        return null;
    }
}

interface UserCardProps {
    data: UserCardData;
}

const UserCard: React.FunctionComponent<UserCardProps> = ({data}) => {
    const [clickCounts, setClickCounts] = useState<Record<number, number>>({});
    const [easterEggRevealed, setEasterEggRevealed] = useState<boolean>();

    const handleAchievementClick = (achievement: Achievement) => {
        setClickCounts(prev => {
            if (achievement.id !== 3) {
                return prev;
            }

            const prevCount = prev[achievement.id] ?? 0;
            const nextCount = prevCount + 1;
            const next = {...prev, [achievement.id]: nextCount};

            if (nextCount === 5) {
                setEasterEggRevealed(() => true);
            }

            return next;
        });
    };

    return (
      <div className="widget">
        {easterEggRevealed ? (
          <FlappyBug
            userId={YTApp.entity?.login} 
            onClose={() => setEasterEggRevealed(false)}
            onScoreSubmit={async (score) => {

              console.log(`onScoreSubmit 1`);
              const userLogin = YTApp.entity?.login;
              console.log(`onScoreSubmit 2`);
              if (!userLogin) {
                  return null;
              }
              console.log(`onScoreSubmit 3`);
              try {
                  console.log(`Submitting score for user ${userLogin}: ${score}`);
                  const response = await host.fetchApp('backend/easter-egg', {
                      body: {
                          userId: userLogin,
                          score: score
                      },
                  });
                  console.log(`Response: ${JSON.stringify(response)}`);
                  return true;
              } catch (error) {
                  console.log(`error: ${JSON.stringify(error)}`);
                  return false;
              }
            }}
          />
            ) : (
              <>
                <div className="level-row">
                  <div className="level-header">
                    <span className="level-label">Level: {data.level}, {data.heroClass}</span>
                    <span className="level-xp">{data.xp} / {data.maxXp}</span>
                  </div>
                  <ProgressBar value={data.xp} max={data.maxXp}/>
                </div>
                <div className="achievements-row">
                  <div className="achievements">
                    {data.achievements.map(achievement => {
                                return (
                                  <div
                                    key={achievement.id}
                                    className="achievement-item"
                                    title={achievement.description}
                                    onClick={() => handleAchievementClick(achievement)}
                                  >
                                    <img
                                      className={`achievement-icon${achievement.count === 0 ? ' achievement-inactive' : ''}`}
                                      src={achievement.imageUrl}
                                      alt={achievement.description}
                                      width={32}
                                      height={32}
                                    />
                                    {achievement.count > 1 && (
                                    <span className="achievement-count">x{achievement.count}</span>
                                        )}
                                  </div>
                                );
                            })}
                  </div>
                </div>
              </>
            )}
      </div>
    );
};

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
