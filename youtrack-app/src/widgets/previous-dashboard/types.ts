export interface Achievement {
    id: number;
    imageUrl: string;
    description: string;
    count: number;
}

export interface UserRating {
    userId: string;
    exp: number;
    level: number;
    heroClass: string;
    achievements: Achievement[];
}

export interface UserRatingResponse {
    items: UserRating[];
    offset: number;
    totalCount: number;
}
