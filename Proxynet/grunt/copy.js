module.exports = {
  tlui: {
    expand: true,
    src: 'tlui/**',
    dest: 'build/'
  },
  images: {
    expand: true,
    cwd: 'src/images/',
    src: '**',
    dest: 'build/images/'
  },
  html: {
    expand: true,
    cwd: 'src/',
    src: 'index.html',
    dest: 'build/'
  }
};