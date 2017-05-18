﻿using Ploeh.AutoFixture.Kernel;

namespace JCore.Foundation.Testing.Commands
{
    public abstract class GenericCommand<T> : ISpecimenCommand where T : class
  {
    public void Execute(object specimen, ISpecimenContext context)
    {
      var castedSpecimen = specimen as T;
      if (castedSpecimen == null)
      {
        return;
      }

      this.ExecuteAction(castedSpecimen, context);
    }

    protected abstract void ExecuteAction(T specimen, ISpecimenContext context);
  }
}