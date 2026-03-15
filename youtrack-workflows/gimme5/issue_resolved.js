const entities = require('@jetbrains/youtrack-scripting-api/entities');
const utils = require('./utils');

exports.rule = entities.Issue.onChange({
  title: 'Gamification event listener',
  guard: (ctx) => {
    const issue = ctx.issue;
    return issue.becomesResolved && issue.fields.Type.name === 'Task';
  },
  action: (ctx) => {
    const payload = utils.buildEventPayloud(ctx, 'ISSUE_RESOLVED');
    
    utils.sendEvent(payload);
  },
  requirements: {
    // TODO: add requirements
  }
});