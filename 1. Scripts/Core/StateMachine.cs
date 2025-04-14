using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KJ
{
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine;
        protected T context;

        public State() { OnInitialize(); }
        internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;
        }

        public virtual void OnInitialize() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }

        public abstract void Update(float deltaTime);

    }

    public sealed class StateMachine<T>
    {
        private T context;
        private State<T> currentState;

        private float elapsedTime = 0.0f;

        private Dictionary<Type, State<T>> states;
        public State<T> CurrentState => currentState;
        public float ElapsedTime => elapsedTime;

        public StateMachine(T context, State<T> initialState)
        {
            Debug.Log("33333");
            states = new Dictionary<Type, State<T>>();
            
            this.context = context;
            
            AddState(initialState);
            currentState = initialState;
        }

        public void AddState(State<T> state)
        {
            state.SetStateMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        public void Update(float deltaTime)
        {
            elapsedTime += deltaTime;

            currentState.Update(deltaTime);
        }

        public R ChangeState<R>() where R : State<T>
        {
            if (currentState.GetType()== typeof(R))
            {
                return currentState as R;
            }

            if (currentState != null)
            {
                currentState.OnStateExit();
            }

            currentState = states[typeof(R)];
            currentState.OnStateEnter();
            elapsedTime = 0.0f;

            return currentState as R;
        }
    }

}
