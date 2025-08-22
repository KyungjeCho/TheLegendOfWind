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
            return "��� [" + content + "] �� ������ϴ�.";
        }
    }
    public class ExpDrop : BaseDrop
    {
        public ExpDrop(string content) : base(content) { }
        public override string Print()
        {
            return "����ġ [" + content + "] �� ������ϴ�.";
        }
    }
    public class ItemDrop : BaseDrop
    {
        public ItemDrop(string content) : base(content) { }
        public override string Print()
        {
            return "[" + content + "] �������� ������ϴ�.";
        }
    }
    public class DropQueue : SingletonMonoBehaviour<DropQueue>
    {
        public UIDropPanel panel;

        private BaseDrop drop;

        public void AddGold(string content)
        {
            drop = new GoldDrop(content);

            panel.AddSlot(drop.Print());
        }

        public void AddExp(string content)
        {
            drop = new ExpDrop(content);

            panel.AddSlot(drop.Print());
        }
        public void ItemDrop(string content)
        {
            drop = new ItemDrop(content);

            panel.AddSlot(drop.Print());
        }
    }
}
