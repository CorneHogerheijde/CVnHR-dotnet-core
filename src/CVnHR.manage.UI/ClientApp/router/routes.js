import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import Settings from 'components/settings'
import Docs from 'components/docs'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  // { name: 'docs-vue', path: '/docs-vue', component: Docs, display: 'Vue documentation', icon: 'book' },
  // { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  // { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Fetch data', icon: 'list' },
  { name: 'settings', path: '/settings', component: Settings, display: 'Settings', icon: 'cog' }
]
