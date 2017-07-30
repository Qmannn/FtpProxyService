"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Lint = require('tslint/lib/lint');
var Rule = (function (_super) {
    __extends(Rule, _super);
    function Rule() {
        _super.apply(this, arguments);
    }
    Rule.prototype.apply = function (sourceFile) {
        return this.applyWithWalker(new CrlfOnlyWalker(sourceFile, this.getOptions()));
    };
    Rule.failureString = 'file must have CR/LF line endings';
    return Rule;
}(Lint.Rules.AbstractRule));
exports.Rule = Rule;
var CrlfOnlyWalker = (function (_super) {
    __extends(CrlfOnlyWalker, _super);
    function CrlfOnlyWalker() {
        _super.apply(this, arguments);
    }
    CrlfOnlyWalker.prototype.visitSourceFile = function (node) {
        if (!this.hasCrlfEndings(node.text)) {
            this.addFailure(this.createFailure(node.pos, node.end - node.pos, Rule.failureString));
        }
        _super.prototype.visitSourceFile.call(this, node);
    };
    CrlfOnlyWalker.prototype.hasCrlfEndings = function (text) {
        var result = true;
        for (var i = 0; i < text.length; i++) {
            var prevCharCode = text.charCodeAt(i - 1);
            var currentCharCode = text.charCodeAt(i);
            var nextCharCode = text.charCodeAt(i + 1);
            if ((currentCharCode === 0x0D && nextCharCode !== 0x0A) ||
                (currentCharCode === 0x0A && prevCharCode !== 0x0D)) {
                result = false;
            }
        }
        return result;
    };
    return CrlfOnlyWalker;
}(Lint.RuleWalker));
//# sourceMappingURL=crlfOnlyRule.js.map