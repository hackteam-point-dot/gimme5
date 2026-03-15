export interface ProjectConfiguration {
  projectId: string;
  issueWeightType: 'None' | 'StoryPoints' | 'Time';
  issueUnitWeight: number;
  issueResolveReward: number;
  bugResolveReward: number;
  issueWeightFieldName: string;
  priorityMultipliers: Record<string, number>;
  achievementRewards: Record<string, number>;
  achievementEnabled: Record<string, boolean>;
}
