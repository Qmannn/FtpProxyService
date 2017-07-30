import * as ts from 'typescript';
import * as Lint from 'tslint/lib/lint';

export class Rule extends Lint.Rules.AbstractRule {
    public static failureString: string = 'private class propery names must start with underscore character';

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithWalker(new ClassMemberNamingWalker(sourceFile, this.getOptions()));
    }
}

class ClassMemberNamingWalker extends Lint.RuleWalker {
    public visitPropertyDeclaration(node: ts.PropertyDeclaration): void {
        const isPrivate: boolean = (Math.floor(node.flags / ts.NodeFlags.Private) % 2) === 1;
        if (isPrivate) {
            if (node.name.getText()[0] !== '_') {
                this.addFailure(this.createFailure(node.name.getStart(), node.name.getWidth(), Rule.failureString));
            }
        }
        super.visitPropertyDeclaration(node);
    }
}
