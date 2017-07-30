module.exports = {
  tlApp: {
    src: [
      [
        'App/*.ts',
        'App/**/*.ts'
      ]
    ],
    out: 'build/typescript.js',
    options: {
        fast: 'never',
        target: 'es5',
        sourceMap: false,
        failOnTypeErrors: true,
        module: 'system',
        moduleResolution: 'node',
        noImplicitAny: true,
        suppressImplicitAnyIndexErrors: true,
        experimentalDecorators: true
    }
  }
};