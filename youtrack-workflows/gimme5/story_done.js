const entities = require('@jetbrains/youtrack-scripting-api/entities');
const utils = require('./utils');

exports.rule = entities.Issue.onChange({
  title: 'Story_resolved',
  guard: (ctx) => {
    const issue = ctx.issue;
    return issue.fields.Type.name === 'User Story' && issue.becomesResolved;
  },
  action: (ctx) => {
    const payload = utils.buildEventPayloud(ctx, 'STORY_DONE');
    
    utils.sendEvent(payload);
  },
  requirements: {
    // TODO: add requirements
  }
});