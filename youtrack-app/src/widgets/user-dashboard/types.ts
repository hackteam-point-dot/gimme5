export interface UserRating {
    userId: string;
    exp: number;
    level: number;
    heroClass: string;
}

export interface UserRatingResponse {
    items: UserRating[];
    offset: number;
    totalCount: number;
}