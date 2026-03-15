import React, {memo, useEffect, useMemo, useState} from 'react';
import Table from '@jetbrains/ring-ui-built/components/table/table';
import Selection from '@jetbrains/ring-ui-built/components/table/selection';
import {type Column} from '@jetbrains/ring-ui-built/components/table/header-cell';

import {type EmbeddableWidgetAPI} from '../../../@types/globals';
import {type UserRating} from './types';

const DEFAULT_LIMIT = 10;
const DEFAULT_SKIP = 0;

const host = await YTApp.register() as EmbeddableWidgetAPI;
await host.setTitle('User Rating', '');

async function fetchLeaderboard(): Promise<UserRating[]> {
    try {
        const result = await host.fetchApp('backend/leaderboard', {
            query: {limit: String(DEFAULT_LIMIT), skip: String(DEFAULT_SKIP)}
        });
        return (result as UserRating[]) ?? [];
    } catch {
        return [];
    }
}

function buildColumns(data: UserRating[]): Column<UserRating>[] {
    return [
        {
            id: 'rank',
            title: '#',
            getValue: (item: UserRating) => data.indexOf(item) + 1,
        },
        {
            id: 'userId',
            title: 'User',
            getValue: (item: UserRating) => item.userId,
        },
        {
            id: 'level',
            title: 'Level',
            getValue: (item: UserRating) => item.level,
        },
        {
            id: 'exp',
            title: 'XP',
            getValue: (item: UserRating) => item.exp.toLocaleString(),
            rightAlign: true,
        },
    ];
}

const AppComponent: React.FunctionComponent = () => {
    const [data, setData] = useState<UserRating[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchLeaderboard().then(result => {
            console.log('Fetched leaderboard data:', result);
            setData(result);
            setLoading(false);
        });
    }, []);

    const columns = useMemo(() => buildColumns(data), [data]);
    const selection = useMemo(() => new Selection<UserRating>({data}), [data]);

    if (loading) {
        return null;
    }

    return (
      <div className="widget">
        <Table
          data={data}
          columns={columns}
          selection={selection}
          selectable={false}
          getItemKey={(item: UserRating) => item.userId}
        />
      </div>
    );
};

export const App = memo(AppComponent);
