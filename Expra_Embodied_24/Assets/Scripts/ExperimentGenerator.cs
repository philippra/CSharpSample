using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;
public class ExperimentGenerator : MonoBehaviour
{
    public GameObject cursor;

    private int TrialsPerPracticeBlock = 24; 
    private int TrialsPerExperimentalBlock = 48;


    public void Start()
    {
        cursor = GameObject.Find("Cursor");
    }

    public void Generate(Session session)
    {
        Block OverlapTrackingPractice = session.CreateBlock(TrialsPerPracticeBlock);

        Block NonOverlapTrackingPractice = session.CreateBlock(TrialsPerPracticeBlock);

        Block OverlapTrackingExperimental1 = session.CreateBlock(TrialsPerExperimentalBlock);

        Block OverlapTrackingExperimental2 = session.CreateBlock(TrialsPerExperimentalBlock);

        Block OverlapTrackingExperimental3 = session.CreateBlock(TrialsPerExperimentalBlock);

        Block NonOverlapTrackingExperimental1 = session.CreateBlock(TrialsPerExperimentalBlock);

        Block NonOverlapTrackingExperimental2 = session.CreateBlock(TrialsPerExperimentalBlock);

        Block NonOverlapTrackingExperimental3 = session.CreateBlock(TrialsPerExperimentalBlock);

        foreach (Block block in session.blocks)
        {
            foreach (Trial trial in block.trials)
            {
                if (block.number == 1 || block.number == 2)
                {
                    if (block.number == 2 && trial.numberInBlock <= (TrialsPerPracticeBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", -1);
                        trial.settings.SetValue("training", 1);
                        trial.settings.SetValue("response_mapping", "non-overlap");
                    }
                    else if (block.number == 2 && trial.numberInBlock > (TrialsPerPracticeBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", 1);
                        trial.settings.SetValue("training", 1);
                        trial.settings.SetValue("response_mapping", "non-overlap");
                    }
                    else if (block.number == 1 && trial.numberInBlock <= (TrialsPerPracticeBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", -1);
                        trial.settings.SetValue("training", 1);
                        trial.settings.SetValue("response_mapping", "overlap");
                    }
                    else
                    {
                        trial.settings.SetValue("pert_direct", 1);
                        trial.settings.SetValue("training", 1);
                        trial.settings.SetValue("response_mapping", "overlap");
                    }
                }

                else if (block.number == 3 || block.number == 4 || block.number == 5)
                {
                    if (trial.numberInBlock <= (TrialsPerExperimentalBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", -1);
                        trial.settings.SetValue("training", 0);
                        trial.settings.SetValue("response_mapping", "overlap");
                    }
                    else if (trial.numberInBlock > (TrialsPerExperimentalBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", 1);
                        trial.settings.SetValue("training", 0);
                        trial.settings.SetValue("response_mapping", "overlap");
                    }
                    
                }

                else if (block.number == 6 || block.number == 7 || block.number == 8)
                {
                    if (trial.numberInBlock <= (TrialsPerExperimentalBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", -1);
                        trial.settings.SetValue("training", 0);
                        trial.settings.SetValue("response_mapping", "non-overlap");
                    }
                    else if (trial.numberInBlock > (TrialsPerExperimentalBlock / 2))
                    {
                        trial.settings.SetValue("pert_direct", 1);
                        trial.settings.SetValue("training", 0);
                        trial.settings.SetValue("response_mapping", "non-overlap");
                    }
                }
            }
        }

        

        foreach (Block block in session.blocks)
        {
            int trialsInBlock = block.trials.Count;
            int halfTrials = trialsInBlock / 2;
            int trialsPerReward = halfTrials / 3; // Set the desired number of trials per reward level

            // Set rewards for the first half of the block
            for (int i = 0; i < halfTrials; i++)
            {
                int rewardValue;
                if (i < trialsPerReward * 1)
                {
                    rewardValue = 70;
                }
                else if (i < trialsPerReward * 2)
                {
                    rewardValue = 30;
                }
                else
                {
                    rewardValue = 50;
                }

                block.trials[i].settings.SetValue("reward_right", rewardValue);
            }

            // Set rewards for the second half of the block
            for (int i = halfTrials; i < trialsInBlock; i++)
            {
                int rewardValue;
                if ((i-halfTrials) < trialsPerReward * 1)
                {
                    rewardValue = 70;
                }
                else if ((i - halfTrials) < trialsPerReward * 2)
                {
                    rewardValue = 30;
                }
                else
                {
                    rewardValue = 50;
                }

                block.trials[i].settings.SetValue("reward_right", rewardValue);
            }

            block.trials.Shuffle();
            block.trials.Shuffle();
        }


        if ((int)session.participantDetails["counterbalance"] == 1)
        {
            // right hand tracking
            session.blocks = new List<Block>();

            session.blocks.Add(OverlapTrackingPractice);
            session.blocks.Add(OverlapTrackingExperimental1);
            session.blocks.Add(OverlapTrackingExperimental2);
            session.blocks.Add(OverlapTrackingExperimental3);

            session.blocks.Add(NonOverlapTrackingPractice);
            session.blocks.Add(NonOverlapTrackingExperimental1);
            session.blocks.Add(NonOverlapTrackingExperimental2);
            session.blocks.Add(NonOverlapTrackingExperimental3);

        }
        else if ((int)session.participantDetails["counterbalance"] == 2)
        {
            // left hand tracking
            session.blocks = new List<Block>();

            session.blocks.Add(OverlapTrackingPractice);
            session.blocks.Add(OverlapTrackingExperimental1);
            session.blocks.Add(OverlapTrackingExperimental2);
            session.blocks.Add(OverlapTrackingExperimental3);

            session.blocks.Add(NonOverlapTrackingPractice);
            session.blocks.Add(NonOverlapTrackingExperimental1);
            session.blocks.Add(NonOverlapTrackingExperimental2);
            session.blocks.Add(NonOverlapTrackingExperimental3);

        }

        else if ((int)session.participantDetails["counterbalance"] == 3)
        {
            // right hand tracking
            session.blocks = new List<Block>();

            session.blocks.Add(NonOverlapTrackingPractice);
            session.blocks.Add(NonOverlapTrackingExperimental1);
            session.blocks.Add(NonOverlapTrackingExperimental2);
            session.blocks.Add(NonOverlapTrackingExperimental3);

            session.blocks.Add(OverlapTrackingPractice);
            session.blocks.Add(OverlapTrackingExperimental1);
            session.blocks.Add(OverlapTrackingExperimental2);
            session.blocks.Add(OverlapTrackingExperimental3);

        }

        else if ((int)session.participantDetails["counterbalance"] == 4)
        {
            // left hand tracking
            session.blocks = new List<Block>();

            session.blocks.Add(NonOverlapTrackingPractice);
            session.blocks.Add(NonOverlapTrackingExperimental1);
            session.blocks.Add(NonOverlapTrackingExperimental2);
            session.blocks.Add(NonOverlapTrackingExperimental3);

            session.blocks.Add(OverlapTrackingPractice);
            session.blocks.Add(OverlapTrackingExperimental1);
            session.blocks.Add(OverlapTrackingExperimental2);
            session.blocks.Add(OverlapTrackingExperimental3);

        }

        else if ((int)session.participantDetails["counterbalance"] == 5)
        {
           // right hand tracking
           session.blocks = new List<Block>();

           session.blocks.Add(OverlapTrackingPractice);
           session.blocks.Add(OverlapTrackingExperimental1);
           session.blocks.Add(OverlapTrackingExperimental2);
           session.blocks.Add(OverlapTrackingExperimental3);

           session.blocks.Add(NonOverlapTrackingPractice);
           session.blocks.Add(NonOverlapTrackingExperimental1);
           session.blocks.Add(NonOverlapTrackingExperimental2);
           session.blocks.Add(NonOverlapTrackingExperimental3);

        }
        else if ((int)session.participantDetails["counterbalance"] == 6)
        {
           // left hand tracking
           session.blocks = new List<Block>();

           session.blocks.Add(OverlapTrackingPractice);
           session.blocks.Add(OverlapTrackingExperimental1);
           session.blocks.Add(OverlapTrackingExperimental2);
           session.blocks.Add(OverlapTrackingExperimental3);

           session.blocks.Add(NonOverlapTrackingPractice);
           session.blocks.Add(NonOverlapTrackingExperimental1);
           session.blocks.Add(NonOverlapTrackingExperimental2);
           session.blocks.Add(NonOverlapTrackingExperimental3);

        }

        else if ((int)session.participantDetails["counterbalance"] == 7)
        {
           // right hand tracking
           session.blocks = new List<Block>();

           session.blocks.Add(NonOverlapTrackingPractice);
           session.blocks.Add(NonOverlapTrackingExperimental1);
           session.blocks.Add(NonOverlapTrackingExperimental2);
           session.blocks.Add(NonOverlapTrackingExperimental3);

           session.blocks.Add(OverlapTrackingPractice);
           session.blocks.Add(OverlapTrackingExperimental1);
           session.blocks.Add(OverlapTrackingExperimental2);
           session.blocks.Add(OverlapTrackingExperimental3);

        }

        else if ((int)session.participantDetails["counterbalance"] == 8)
        {
           // left hand tracking
           session.blocks = new List<Block>();

           session.blocks.Add(NonOverlapTrackingPractice);
           session.blocks.Add(NonOverlapTrackingExperimental1);
           session.blocks.Add(NonOverlapTrackingExperimental2);
           session.blocks.Add(NonOverlapTrackingExperimental3);

           session.blocks.Add(OverlapTrackingPractice);
           session.blocks.Add(OverlapTrackingExperimental1);
           session.blocks.Add(OverlapTrackingExperimental2);
           session.blocks.Add(OverlapTrackingExperimental3);

        }

        else
        {
            session.End();
            Application.Quit();
        }

        var pert1_30 = 0;
        var pert1_70 = 0;
        var pert1_50 = 0;

        var trials_response_mapping_o = 0;
        var trials_response_mapping_no = 0;

        foreach (Block block in session.blocks)
        {
            foreach (Trial trial in block.trials)

            {

                if (trial.settings.GetInt("reward_right") == 70 & trial.settings.GetInt("pert_direct") == 1)
                    pert1_70 += 1;
                else if (trial.settings.GetInt("reward_right") == 30 & trial.settings.GetInt("pert_direct") == 1)
                    pert1_30 += 1;
                else if (trial.settings.GetInt("reward_right") == 50 & trial.settings.GetInt("pert_direct") == 1)
                    pert1_50 += 1;

                if (trial.settings.GetString("response_mapping") == "overlap")
                    trials_response_mapping_o += 1;
                else if (trial.settings.GetString("response_mapping") == "non-overlap")
                    trials_response_mapping_no += 1;

               }
        }
        
        Debug.Log("Pert right 30 reward: " + pert1_30);
        Debug.Log("Pert right 50 reward: " + pert1_50);
        Debug.Log("Pert right 70 reward: " + pert1_70);
        Debug.Log("trials_response_mapping_o: " + trials_response_mapping_o);
        Debug.Log("trials_response_mapping_no: " + trials_response_mapping_no);
        Debug.Log("Number of blocks: " + session.blocks.Count);
    }
}
