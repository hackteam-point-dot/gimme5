import React, {memo, useState, useCallback} from 'react';
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
];

const AppComponent: React.FunctionComponent = () => {
  const [selection, setSelection] = useState(
    () => new Selection<UserRating>({data: mockUsers}),
  );

  const handleSelectionChange = useCallback(
    (newSelection: Selection<UserRating>) => setSelection(newSelection),
    [],
  );

  return (
    <div className="widget">
      <Table
        data={mockUsers}
        columns={columns}
        selection={selection}
        onSelect={handleSelectionChange}
        getItemKey={(item: UserRating) => item.id}
      />
    </div>
  );
};

export const App = memo(AppComponent);
