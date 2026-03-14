export interface Achievement {
  id: number;
  imageUrl: string;
  description: string;
}

export interface UserCardData {
  xp: number;
  maxXp: number;
  level: number;
  balance: number;
  achievements: Achievement[];
}
