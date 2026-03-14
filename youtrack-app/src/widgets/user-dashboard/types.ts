import {type Achievement} from '../user-card/types';

export interface UserRating {
  id: number;
  username: string;
  team: string;
  xp: number;
  achievements: Achievement[];
}

export type {Achievement};
