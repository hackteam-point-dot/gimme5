import {type UserRating} from './types';

export const mockUsers: UserRating[] = [
  {id: 1, username: 'Alice Johnson', team: 'Backend', xp: 4850, achievements: [
    {id: 1, imageUrl: 'https://via.placeholder.com/24', description: 'First commit', count: 1},
    {id: 2, imageUrl: 'https://via.placeholder.com/24', description: 'Bug hunter', count: 3},
  ]},
  {id: 2, username: 'Bob Smith', team: 'Frontend', xp: 4200, achievements: [
    {id: 1, imageUrl: 'https://via.placeholder.com/24', description: 'First commit', count: 1},
  ]},
  {id: 3, username: 'Charlie Lee', team: 'Backend', xp: 3900, achievements: [
    {id: 2, imageUrl: 'https://via.placeholder.com/24', description: 'Bug hunter', count: 2},
    {id: 3, imageUrl: 'https://via.placeholder.com/24', description: 'Team player', count: 1},
  ]},
  {id: 4, username: 'Diana Petrova', team: 'QA', xp: 3750, achievements: [
    {id: 1, imageUrl: 'https://via.placeholder.com/24', description: 'First commit', count: 1},
    {id: 2, imageUrl: 'https://via.placeholder.com/24', description: 'Bug hunter', count: 5},
    {id: 3, imageUrl: 'https://via.placeholder.com/24', description: 'Team player', count: 2},
  ]},
  {id: 5, username: 'Evan Wright', team: 'DevOps', xp: 3500, achievements: [
    {id: 3, imageUrl: 'https://via.placeholder.com/24', description: 'Team player', count: 1},
  ]},
  {id: 6, username: 'Fiona Chen', team: 'Frontend', xp: 3100, achievements: []},
  {id: 7, username: 'George Kim', team: 'QA', xp: 2800, achievements: [
    {id: 1, imageUrl: 'https://via.placeholder.com/24', description: 'First commit', count: 1},
  ]},
  {id: 8, username: 'Hannah Davis', team: 'Backend', xp: 2650, achievements: [
    {id: 2, imageUrl: 'https://via.placeholder.com/24', description: 'Bug hunter', count: 1},
  ]},
  {id: 9, username: 'Ivan Novak', team: 'DevOps', xp: 2400, achievements: []},
  {id: 10, username: 'Julia Martinez', team: 'Frontend', xp: 2100, achievements: [
    {id: 1, imageUrl: 'https://via.placeholder.com/24', description: 'First commit', count: 1},
    {id: 3, imageUrl: 'https://via.placeholder.com/24', description: 'Team player', count: 1},
  ]},
];
