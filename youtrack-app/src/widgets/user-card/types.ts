export interface Achievement {
    id: number;
    imageUrl: string;
    description: string;
    count: number;
}

export interface UserCardData {
    xp: number;
    maxXp: number;
    level: number;
    heroClass: string;
    balance: number;
    achievements: Achievement[];
}
