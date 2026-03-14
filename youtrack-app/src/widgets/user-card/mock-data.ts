import {type Achievement} from './types';

export const mockAchievements: Achievement[] = [
  {
    id: 1,
    imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=first-blood&size=48',
    description: 'First Blood — resolved the first issue in the sprint',
  },
  {
    id: 2,
    imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=speed-demon&size=48',
    description: 'Speed Demon — closed 5 issues in one day',
  },
  {
    id: 3,
    imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=bug-hunter&size=48',
    description: 'Bug Hunter — found and reported 10 bugs',
  },
  {
    id: 4,
    imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=team-player&size=48',
    description: 'Team Player — reviewed 15 pull requests',
  },
  {
    id: 5,
    imageUrl: 'https://api.dicebear.com/9.x/shapes/svg?seed=streak-master&size=48',
    description: 'Streak Master — completed tasks 7 days in a row',
  },
];
