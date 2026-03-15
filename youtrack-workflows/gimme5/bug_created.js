const entities = require('@jetbrains/youtrack-scripting-api/entities');
const utils = require('./utils');

exports.rule = entities.Issue.onChange({
  title: 'Bug_created',
  guard: (ctx) => {
    const issue = ctx.issue;
    return (issue.fields.Type.name === 'Bug' || issue.fields.Type.name === 'Sub bug') && issue.becomesReported;
  },
  action: (ctx) => {
    const payload = utils.buildEventPayloud(ctx, 'BUG_CREATED');
    
    utils.sendEvent(payload);
  },
  requirements: {
    // TODO: add requirements
  }
});