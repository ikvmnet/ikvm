export default defineAppConfig({
  github: {
    owner: 'ikvmnet',
    repo: 'ikvm',
    branch: 'main'
  },
  docus: {
    title: 'IKVM',
    description: 'A Java Virtual Machine and Bytecode-to-IL Converter for .NET.',
    url: 'http://ikvm.org',
    socials: {
      github: 'ikvmnet/ikvm'
    },
    github: {
      dir: '.starters/default/content',
      branch: 'main',
      repo: 'docus',
      owner: 'nuxt-themes',
      edit: true
    },
    aside: {
      level: 0,
      collapsed: false,
      exclude: []
    },
    main: {
      padded: true,
      fluid: true
    },
    header: {
      showLinkIcon: false,
      exclude: [],
      fluid: true
    },
    footer: {
      credits: {
        text: 'Copyright 2023 Jerome Haltom',
        href: 'http://ikvm.org'
      },
      icons: [],
    },
  }
})
