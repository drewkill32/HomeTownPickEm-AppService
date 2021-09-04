import { AzureFunction, Context } from '@azure/functions';
import { updatePicks } from '../utils/agent';

const timerTrigger: AzureFunction = async function (
  context: Context,
  myTimer: any
): Promise<void> {
  const token = await updatePicks();
  console.log('updated Picks!');
};

export default timerTrigger;
