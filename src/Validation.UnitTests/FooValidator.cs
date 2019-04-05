using System;
using System.Collections.Generic;

namespace Kraphity.Validation.UnitTests
{
    internal class FooValidator : Validator<Foo>
    {
        public FooValidator(IEnumerable<IValidationRule<Foo>> rules)
            : base()
        {
            foreach(var rule in rules)
            {
                this.AddRule(rule);
            }
        }

        public FooValidator()
        {
        }

        public new IMemberValidationRule<Foo, TMember> For<TMember>(Func<Foo, TMember> member, string name)
        {
            return base.For(member, name);
        }

        public new void AddRule(IValidationRule<Foo> rule)
        {
            base.AddRule(rule);
        }
    }
}