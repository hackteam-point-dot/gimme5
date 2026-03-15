export interface UserRating {
    userId: string;
    exp: number;
    level: number;
}

export interface UserRatingResponse {
    items: UserRating[];
    offset: number;
    totalCount: number;
}