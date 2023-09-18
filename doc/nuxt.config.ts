export default defineNuxtConfig({
  extends: '@nuxt-themes/docus',
  modules: [
    '@nuxtjs/plausible',
    '@nuxt/devtools'
  ],
  content: {
    documentDriven: true
  }
})
