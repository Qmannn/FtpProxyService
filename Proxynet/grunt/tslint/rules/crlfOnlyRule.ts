import * as ts from 'typescript';
import * as Lint from 'tslint/lib/lint';

export class Rule extends Lint.Rules.AbstractRule {
    public static failureString: string = 'file must have CR/LF line endings';

    public apply(sourceFile: ts.SourceFile): Lint.RuleFailure[] {
        return this.applyWithWalker(new CrlfOnlyWalker(sourceFile, this.getOptions()));
    }
}

class CrlfOnlyWalker extends Lint.RuleWalker {
    public visitSourceFile(node: ts.SourceFile): void {
        if (!this.hasCrlfEndings(node.text)) {
            this.addFailure(this.createFailure(node.pos, node.end - node.pos, Rule.failureString));
        }
        super.visitSourceFile(node);
    }

    private hasCrlfEndings(text: string): boolean {
        let result: boolean = true;
        for (let i: number = 0; i < text.length; i ++) {
            const prevCharCode: number = text.charCodeAt(i - 1);
            const currentCharCode: number = text.charCodeAt(i);
            const nextCharCode: number = text.charCodeAt(i + 1);
            if ((currentCharCode === 0x0D && nextCharCode !== 0x0A) ||
                (currentCharCode === 0x0A && prevCharCode !== 0x0D)) {
                result = false;
            }
        }
        return result;
    }
}
