using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyAdmin.Scripts
{
    public static class Macros
    {
            static Macros()
            {
                // Можно прописать горячие клавиши по умолчанию
                InputGestureCollection inputs = new InputGestureCollection();
                inputs.Add(new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S"));

                SomeCommand = new RoutedUICommand("Some", "SomeCommand", typeof(Macros), inputs);
            }

            public static RoutedCommand SomeCommand { get; private set; }
        }
    }
}

