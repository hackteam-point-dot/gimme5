import React, {memo} from 'react';
import Table from '@jetbrains/ring-ui-built/components/table/table';
import Selection from '@jetbrains/ring-ui-built/components/table/selection';
import {type Column} from '@jetbrains/ring-ui-built/components/table/header-cell';

import {type EmbeddableWidgetAPI} from '../../../@types/globals';
import {type UserRating} from './types';
import {mockUsers} from './mock-data';

const host = await YTApp.register() as EmbeddableWidgetAPI;
await host.setTitle('User Rating', '');

const columns: Column<UserRating>[] = [
  {
    id: 'rank',
    title: '#',
    getValue: (item: UserRating) => mockUsers.indexOf(item) + 1,
  },
  {
    id: 'username',
    title: 'User',
    getValue: (item: UserRating) => item.username,
  },
  {
    id: 'team',
    title: 'Team',
    getValue: (item: UserRating) => item.team,
  },
  {
    id: 'xp',
    title: 'XP',
    getValue: (item: UserRating) => item.xp.toLocaleString(),
    rightAlign: true,
  },
  {
    id: 'achievements',
    title: 'Achievements',
    getValue: (item: UserRating) => (
      <div className="achievements">
        {item.achievements.map(a => (
          <div key={a.id} className="achievement-item" title={a.description}>
            <img className="achievement-icon" src={a.imageUrl} alt={a.description} width={24} height={24}/>
            {a.count > 1 && <span className="achievement-count">{a.count}</span>}
          </div>
        ))}
      </div>
    ),
  },
];

const selection = new Selection<UserRating>({data: mockUsers});

const AppComponent: React.FunctionComponent = () => (
  <div className="widget">
    <Table
      data={mockUsers}
      columns={columns}
      selection={selection}
      selectable={false}
      getItemKey={(item: UserRating) => item.id}
    />
  </div>
);

export const App = memo(AppComponent);
