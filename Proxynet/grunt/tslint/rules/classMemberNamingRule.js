"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ts = require('typescript');
var Lint = require('tslint/lib/lint');
var Rule = (function (_super) {
    __extends(Rule, _super);
    function Rule() {
        _super.apply(this, arguments);
    }
    Rule.prototype.apply = function (sourceFile) {
        return this.applyWithWalker(new ClassMemberNamingWalker(sourceFile, this.getOptions()));
    };
    Rule.failureString = 'private class propery names must start with underscore character';
    return Rule;
}(Lint.Rules.AbstractRule));
exports.Rule = Rule;
var ClassMemberNamingWalker = (function (_super) {
    __extends(ClassMemberNamingWalker, _super);
    function ClassMemberNamingWalker() {
        _super.apply(this, arguments);
    }
    ClassMemberNamingWalker.prototype.visitPropertyDeclaration = function (node) {
        var isPrivate = (Math.floor(node.flags / ts.NodeFlags.Private) % 2) === 1;
        if (isPrivate) {
            if (node.name.getText()[0] !== '_') {
                this.addFailure(this.createFailure(node.name.getStart(), node.name.getWidth(), Rule.failureString));
            }
        }
        _super.prototype.visitPropertyDeclaration.call(this, node);
    };
    return ClassMemberNamingWalker;
}(Lint.RuleWalker));
//# sourceMappingURL=classMemberNamingRule.js.map