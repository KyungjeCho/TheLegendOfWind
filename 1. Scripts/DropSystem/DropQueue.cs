using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    public abstract class BaseDrop
    {
        protected string content;

        public string Content => content;

        public BaseDrop(string content)
        { 
            this.content = content; 
        }

        public abstract string Print();
    }
    public class GoldDrop : BaseDrop
    {
        public GoldDrop(string content) : base(content) { }
        public override string Print()
        {
            return "골드 [" + content + "] 를 얻었습니다.";
        }
    }
    public class ExpDrop : BaseDrop
    {
        public ExpDrop(string content) : base(content) { }
        public override string Print()
        {
            return "경험치 [" + content + "] 를 얻었습니다.";
        }
    }
    public class ItemDrop : BaseDrop
    {
        public ItemDrop(string content) : base(content) { }
        public override string Print()
        {
            return "[" + content + "] 아이탬을 얻었습니다.";
        }
    }
    public class DropQueue : SingletonMonoBehaviour<DropQueue>
    {
        private Queue<BaseDrop> queue = new Queue<BaseDrop>();

        public Queue<BaseDrop> Queue => queue;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
