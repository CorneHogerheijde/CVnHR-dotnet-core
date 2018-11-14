// import axios from 'axios'

const searchKvkApiType = 'searchKvkApiType'

const kvkApiSearchActions = {
  searchKvkApi: async ({ commit }, q) => {
    commit(searchKvkApiType, q)
  }
}

const kvkApiSearchMutations = {
  async [searchKvkApiType] (state, q) {
    console.log('searching q=', q)

    state.kvkApiSearch.q = q
  }
}

export { kvkApiSearchActions, kvkApiSearchMutations }
