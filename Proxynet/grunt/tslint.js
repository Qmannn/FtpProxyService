var configuration = {
    "rules": {
        "align": [
            false,
            "parameters",
            "arguments",
            "statements"
        ],
        "ban": false,
        "class-name": true,
        "comment-format": [true, "check-space", "check-uppercase"],
        "curly": true,
        "eofline": false,
        "forin": true,
        "indent": [true, 4],
        "interface-name": true,
        "jsdoc-format": true,
        "label-position": true,
        "label-undefined": true,
        "max-line-length": [false, 140],
        "member-ordering": [
            false,
            "public-before-private",
            "static-before-instance",
            "variables-before-functions"
        ],
        "no-any": false,
        "no-arg": true,
        "no-bitwise": true,
        "no-console": [
            true,
            "log",
            "debug",
            "time",
            "timeEnd",
            "trace"
        ],
        "no-consecutive-blank-lines": true,
        "no-construct": true,
        "no-constructor-vars": true,
        "no-debugger": true,
        "no-duplicate-key": true,
        "no-duplicate-variable": true,
        "no-empty": false,
        "no-eval": true,
        "no-string-literal": false,
        "no-switch-case-fall-through": true,
        "no-trailing-whitespace": true,
        "no-unreachable": true,
        "no-unused-expression": false,
        "no-unused-variable": true,
        "no-use-before-declare": true,
        "no-var-keyword": false,
        "no-var-requires": true,
        "one-line": [
            true,
            "check-open-brace",
            "check-catch",
            "check-else",
            "check-whitespace"
        ],
        "quotemark": [true, "single"],
        "radix": true,
        "semicolon": true,
        "switch-default": true,
        "triple-equals": [true, "allow-null-check"],
        "typedef": [
            true,
            "call-signature",
            "parameter",
            "property-declaration",
            "variable-declaration",
            "member-variable-declaration"
        ],
        "typedef-whitespace": [
            true,
            {
                "call-signature": "nospace",
                "index-signature": "nospace",
                "parameter": "nospace",
                "property-declaration": "nospace",
                "variable-declaration": "nospace"
            }
        ],
        "use-strict": [
            false,
            "check-module",
            "check-function"
        ],
        "variable-name": [true, "allow-leading-underscore"],
        "whitespace": [
            true,
            "check-branch",
            "check-decl",
            "check-operator",
            "check-module",
            "check-separator",
            "check-type"
        ],
        "member-access": true
    }
};
module.exports = {
    options: {
        configuration: configuration,
        rulesDirectory: 'grunt/tslint/rules/',
        formatter: 'msbuild'
    },
    files: {
        src: [
            'App/*.ts',
            'App/**/*.ts'
        ]
    }
};