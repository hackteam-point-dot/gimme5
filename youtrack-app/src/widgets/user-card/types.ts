export interface Achievement {
  id: number;
  imageUrl: string;
  description: string;
}

export interface UserCardData {
  xp: number;
  balance: number;
  achievements: Achievement[];
}
