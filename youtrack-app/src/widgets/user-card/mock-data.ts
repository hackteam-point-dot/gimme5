import {type UserCardData} from './types';

export const mockUserCardData: UserCardData = {
  xp: 1250,
  maxXp: 2000,
  level: 5,
  balance: 500,
  achievements: [
    {
      id: 1,
      imageUrl: 'data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNCAyNCIgd2lkdGg9IjI0IiBoZWlnaHQ9IjI0IiBmaWxsPSJub25lIiBzdHJva2U9IiMyQzU4NzciIHN0cm9rZS13aWR0aD0iMiIgc3Ryb2tlLWxpbmVjYXA9InJvdW5kIiBzdHJva2UtbGluZWpvaW49InJvdW5kIj4KICA8Y2lyY2xlIGN4PSIxMiIgY3k9IjEyIiByPSIxMCIgZmlsbD0iI0YyRERCRCIvPgogIDxjaXJjbGUgY3g9IjEyIiBjeT0iMTIiIHI9IjYiIHN0cm9rZT0iIzg1QzNENyIvPgogIDxjaXJjbGUgY3g9IjEyIiBjeT0iMTIiIHI9IjIiIGZpbGw9IiNFODgxNDUiIHN0cm9rZT0iI0U4ODE0NSIvPgogIDxsaW5lIHgxPSIxMiIgeTE9IjIiIHgyPSIxMiIgeTI9IjQiIHN0cm9rZT0iIzJDNTg3NyIvPgogIDxsaW5lIHgxPSIxMiIgeTE9IjIwIiB4Mj0iMTIiIHkyPSIyMiIgc3Ryb2tlPSIjMkM1ODc3Ii8+CiAgPGxpbmUgeDE9IjIiIHkxPSIxMiIgeDI9IjQiIHkyPSIxMiIgc3Ryb2tlPSIjMkM1ODc3Ii8+CiAgPGxpbmUgeDE9IjIwIiB5MT0iMTIiIHgyPSIyMiIgeTI9IjEyIiBzdHJva2U9IiMyQzU4NzciLz4KPC9zdmc+',
      description: 'First Blood — resolved the first issue in the sprint',
    },
    {
      id: 2,
      imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=speed-demon&size=24',
      description: 'Speed Demon — closed 5 issues in one day',
    },
    {
      id: 3,
      imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=bug-hunter&size=24',
      description: 'Bug Hunter — found and reported 10 bugs',
    },
    {
      id: 4,
      imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=team-player&size=24',
      description: 'Team Player — reviewed 15 pull requests',
    },
    {
      id: 5,
      imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=streak-master&size=24',
      description: 'Streak Master — completed tasks 7 days in a row',
    },
  ],
};
