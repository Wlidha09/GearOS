using WindowsInput;
using WindowsInput.Native;
using GearOS.Models;
using System;
using System.Threading.Tasks;

namespace GearOS.Utilities
{
    public class MappingEngine
    {
        private readonly InputSimulator _sim = new InputSimulator();

        public async Task ExecuteMapping(KeyMapping mapping)
        {
            if (mapping == null) return;

            switch (mapping.Type)
            {
                case MappingType.Remap:
                    if (Enum.TryParse(mapping.TargetAction, out VirtualKeyCode key))
                    {
                        _sim.Keyboard.KeyPress(key);
                    }
                    break;

                case MappingType.Macro:
                    await ExecuteMacroSequence(mapping.TargetAction);
                    break;
            }
        }

        private async Task ExecuteMacroSequence(string sequence)
        {
            if (string.IsNullOrEmpty(sequence)) return;

            var steps = sequence.Split(',');
            foreach (var step in steps)
            {
                if (step.StartsWith("DELAY:"))
                {
                    string[] parts = step.Split(':');
                    if (parts.Length > 1 && int.TryParse(parts[1], out int ms))
                    {
                        await Task.Delay(ms);
                    }
                }
                else
                {
                    _sim.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
                }
            }
        }
    }
}