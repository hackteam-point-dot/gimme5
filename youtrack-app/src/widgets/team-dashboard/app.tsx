import React, {memo} from 'react';
import Table from '@jetbrains/ring-ui-built/components/table/table';
import Selection from '@jetbrains/ring-ui-built/components/table/selection';
import {type Column} from '@jetbrains/ring-ui-built/components/table/header-cell';

import {type EmbeddableWidgetAPI} from '../../../@types/globals';
import {type TeamRating} from './types';
import {mockTeams} from './mock-data';

const host = await YTApp.register() as EmbeddableWidgetAPI;
await host.setTitle('Team Rating', '');

const columns: Column<TeamRating>[] = [
  {
    id: 'rank',
    title: '#',
    getValue: (item: TeamRating) => mockTeams.indexOf(item) + 1,
  },
  {
    id: 'team',
    title: 'Team',
    getValue: (item: TeamRating) => item.team,
  },
  {
    id: 'members',
    title: 'Members',
    getValue: (item: TeamRating) => item.members,
    rightAlign: true,
  },
  {
    id: 'xp',
    title: 'XP',
    getValue: (item: TeamRating) => item.xp.toLocaleString(),
    rightAlign: true,
  },
];

const selection = new Selection<TeamRating>({data: mockTeams});

const AppComponent: React.FunctionComponent = () => (
  <div className="widget">
    <Table
      data={mockTeams}
      columns={columns}
      selection={selection}
      selectable={false}
      getItemKey={(item: TeamRating) => item.id}
    />
  </div>
);

export const App = memo(AppComponent);
