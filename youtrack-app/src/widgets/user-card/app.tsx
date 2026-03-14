import React, {memo} from 'react';
import ProgressBar from '@jetbrains/ring-ui-built/components/progress-bar/progress-bar';
import {type UserCardData} from './types';
import {mockUserCardData} from './mock-data';

await YTApp.register();

//const host = await YTApp.register() as HostAPI;

// async function fetchUserCardData(): Promise<UserCardData | null> {
//     try {
//         const userId = YTApp.entity?.id;
//         if (!userId) {return null;}
//         return await host.fetchApp<UserCardData>(`backend/users/${userId}/card`);
//     } catch {
//         return null;
//     }
// }

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
              width={36}
              height={36}
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
    if (mockUserCardData) {
        return <UserCard data={mockUserCardData}/>;
    }
    return <div>The user is not participating in GiveMeFive challenges yet 😿</div>;
};

// const AppComponent: React.FunctionComponent = () => {
//   const [data, setData] = useState<UserCardData | null>(null);
//
//   useEffect(() => {
//     fetchUserCardData().then(result => {
//       if (result) {
//         setData(result);
//       } else {
//         host.closeWidget();
//       }
//     });
//   }, []);
//
//   if (!data) return null;
//
//   return <UserCard data={data}/>;
// };

export const App = memo(AppComponent);
