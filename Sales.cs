﻿using System;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.API;

namespace LPXRemastered
{
    public class Sales
    {
        public bool salesStart = false;
        private DateTime lastSale;
        private DateTime nextSale;
        private DateTime SaleEndTime;
        private DateTime SaleTime;
        private DateTime lastMsg = DateTime.Now;
        byte b = 3;
        public void StartSale()
        {
            LPXRemastered sale = LPXRemastered.Instance;
            SaleTime = DateTime.Now;
            salesStart = true;
            SaleEndTime = SaleTime.AddMinutes(sale.Configuration.Instance.SaleTime);
        }
        public void ResetSale()
        {
            lastSale = DateTime.Now;
            double Random = (double)UnityEngine.Random.Range(LPXRemastered.Instance.Configuration.Instance.minNextSaleTime, (LPXRemastered.Instance.Configuration.Instance.maxNextSaleTime + 1));
            nextSale = lastSale.AddMinutes(Random);
            lastMsg = DateTime.Now;
            salesStart = false;
            Logger.Log("The next sale will start at " + nextSale, ConsoleColor.Green);
        }
        public void MsgSale(IRocketPlayer call)
        {
            TimeSpan time = nextSale - DateTime.Now;
            double timeD = Math.Round(time.TotalMinutes, 1);
            if (time.TotalMinutes >= 1.0 && !salesStart)
                UnturnedChat.Say(call, "The next sale will start in " + timeD + " Minutes");
            else if (time.TotalMinutes < 1 && !salesStart)
                UnturnedChat.Say(call, "The next sale will start in " + (Math.Round(time.TotalSeconds)) + " Seconds");
            else if (time.TotalSeconds <= 0 && salesStart)
                UnturnedChat.Say("Sale have already started");
            return;
        }
        public void CheckSale()
        {
            LPXRemastered sale = LPXRemastered.Instance;
            if (!salesStart )
            {
                if ((nextSale - DateTime.Now).Seconds <= 60 && (DateTime.Now - lastMsg).Seconds >= 60)
                {
                    UnturnedChat.Say("Sale is starting in 1 minute");
                    lastMsg = DateTime.Now;
                }                
                if ((nextSale - DateTime.Now).TotalSeconds <= b && (DateTime.Now - lastMsg).TotalSeconds >= 1)
                {                    
                    if (b != 0)
                    {
                        UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_start", b));
                        lastMsg = DateTime.Now;
                        b -= (byte)1;
                    }
                    else if (!salesStart)
                    {
                        UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_started",sale.Configuration.Instance.SaleTime));
                        lastMsg = DateTime.Now;
                        StartSale();
                        Logger.LogWarning("Sales have started");
                    }
                }
            }
            if (salesStart)
            {
                b = 3;
                if ((SaleEndTime - DateTime.Now).TotalMinutes <= 1 && (SaleEndTime - DateTime.Now).TotalSeconds > 59 && (DateTime.Now - lastMsg).Seconds >= 60)
                {
                    UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_ending", 1, "minute"));
                    lastMsg = DateTime.Now;
                }
                if ((SaleEndTime - DateTime.Now).TotalSeconds <= 10 && (SaleEndTime - DateTime.Now).TotalSeconds > 9 && (DateTime.Now - lastMsg).Seconds >= 10)
                {
                    UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_ending", 10, "seconds"));
                    lastMsg = DateTime.Now;
                }
                if ((SaleEndTime - DateTime.Now).TotalSeconds <= 5 && (SaleEndTime - DateTime.Now).TotalSeconds > 4 && (DateTime.Now - lastMsg).Seconds >= 5)
                {
                    UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_ending", 5, "seconds"));
                    lastMsg = DateTime.Now;
                }
                if (DateTime.Now >= SaleEndTime)
                {
                    UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_end"));
                    Logger.LogWarning("Sales have ended");
                    lastMsg = DateTime.Now;
                    ResetSale();
                }
            }
        }
        
    }

}
