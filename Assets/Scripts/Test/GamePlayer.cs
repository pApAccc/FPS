using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
using VContainer.Unity;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class GamePlayer : IStartable, ITickable
    {
        int num;

        [Inject]
        private ITest test;

        public void Start()
        {
            test.TestFunc();
        }

        public void Tick()
        {

        }

        private void Test(IObjectResolver objectResolver)
        {

        }
        //[Inject]
        //private void Hello(GameManager gameManager)
        //{
        //    num = gameManager.Print();
        //}



        //[Inject]
        //public GamePlayer(GameManager gameManager)
        //{
        //    this.gameManager = gameManager;
        //}



    }
}
