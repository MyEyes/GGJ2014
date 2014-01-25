using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Otherworld.State
{
    public class StateManager
    {
        private Stack<BaseGameState>                stack       = new Stack<BaseGameState>();
        private Queue<StateCommand>                 cmds        = new Queue<StateCommand>();
        private Dictionary<string, BaseGameState>   states;

        public StateManager (Dictionary<string, BaseGameState> states)
        {
            this.states = states;
        }

        public void Load()
        {
            foreach (var screen in states.Values)
            {
                screen.Load();
            }
        }

        public void Update (float dt)
        {
            var input = stack.Reverse().FirstOrDefault (s => s.AcceptsInput);
            if (input != null)
                input.HandleInput (dt);

            foreach (var state in stack)
                state.Update (dt);

            processCommands();
        }

        public void Draw()
        {
            foreach (var screen in stack.Reverse())
            {
                screen.Draw();

                if (!screen.IsOverlay)
                    break;
            }
        }

        public void Push (String name)
        {
            cmds.Enqueue (
                new StateCommand (StateCommand.CommandType.Push, name));
        }

        public void Pop()
        {
            cmds.Enqueue (
                new StateCommand (StateCommand.CommandType.Pop));
        }

        public void Set (String name)
        {
            cmds.Enqueue (
                new StateCommand (StateCommand.CommandType.Set, name));
        }

        private void processCommands()
        {
            if (cmds.Count == 0)
                return;

            int headerPtr = stack.Count;

            var pushCmd = new Action<String> (s =>
            {
                Console.WriteLine ("Pushing " + s);
                var state = states[s];
                stack.Push (state);
            });

            var setCmd = new Action<String> (s =>
            {
                var state = states[s];
                clearStack();
                stack.Push (state);
                headerPtr = 0;
            });

            var popCmd = new Action (() =>
            {
                Console.WriteLine ("Popping");
                stack.Pop();
                headerPtr--;
            });


            Console.WriteLine ("Processing " + cmds.Count + " commands");
            while (cmds.Count > 0)
            {
                var cmd = cmds.Dequeue();
                switch (cmd.Cmd)
                {
                    case StateCommand.CommandType.Set:  setCmd (cmd.Name);  break;
                    case StateCommand.CommandType.Push: pushCmd (cmd.Name); break;
                    case StateCommand.CommandType.Pop:  popCmd();           break;
                }
            }

            Console.WriteLine ("Activating " + headerPtr + " to " + stack.Count);
            for (var i = 0; i < stack.Count - headerPtr; i++)
            {
                stack.ElementAt (i).Activate();
            }
        }

        private void clearStack()
        {
            while (stack.Count > 0)
            {
                stack.Peek().Deactivate();
                stack.Pop();
            }
        }
    }

    public struct StateCommand
    {
        public StateCommand (CommandType cmd, String name = null)
        {
            this.Cmd = cmd;
            this.Name = name;
        }

        public readonly string Name;
        public readonly CommandType Cmd;

        public enum CommandType
        {
            Push,
            Pop,
            Set
        }
    }
}
