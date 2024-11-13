using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Commands
{
    public abstract class BaseAsyncCommand : BaseCommand
    {
        public override async void Execute(object? parameter)
        {
            try
            {
                await ExecuteAsync(parameter);

            }
            catch (Exception) { throw; }
        }

        public abstract Task ExecuteAsync(object parameter);
    }
}
