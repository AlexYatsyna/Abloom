﻿using Abloom.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Actors.Processors
{
    internal class GetDataProcessor : UntypedActor
    {
        private string Hash { get; set; }
        private int PassLength { get; set; }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Get data":
                    GetData();
                    Context.ActorSelection("../display-processor").Tell(new SetInitialData(Hash, PassLength));
                    Sender.Tell("Display");
                    break;
            }
        }

        private void GetData()
        {
            Console.WriteLine("Enter password hash:");
            Hash = Console.ReadLine();

            Console.WriteLine("Enter password length:");
            PassLength = Convert.ToInt32(Console.ReadLine());
        }
    }
}