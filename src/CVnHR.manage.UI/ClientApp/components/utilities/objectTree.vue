<template>
    <div v-if="item">
      <div v-for="(value, label) in item">
        <objectTree :label="label" :value="value" :depth="0"></objectTree>
      </div>
    </div>
    <div v-else-if="label" :style="indent">
      
      <label @click="toggleChildren" :class="collapser" v-if="(value || showEmpty)">
        <icon v-if="isObject(value)" :icon="iconClasses" />
        {{ label }}:
        <span v-if="!isObject(value)">
          {{ formatValue }}
        </span>
      </label>
      <template v-if="showChildren && isObject(value)">
        <template v-if="!isArray(value)">
          <objectTree v-for="(val, label) in value"
                      :label="label"
                      :value="val"
                      :key="label + depth"
                      :depth="depth + 1"></objectTree>
        </template>
        <template v-else>
          <template v-for="(val, index) in value">
            [
            <template v-if="isObject(val)">
              <objectTree v-for="(val, label) in val"
                          :label="label"
                          :value="val"
                          :key="label + depth + index"
                          :depth="depth + 1"></objectTree>
            </template>
            <template v-else>
              <span>"{{val}}"</span>
            </template>
            ]
            <span v-if="index != Object.keys(value).length - 1">,</span>
          </template>
        </template>
      </template>
    </div>
</template>

<script>
  import { mapActions, mapState } from 'vuex'
  
  export default {
    props: ['label', 'value', 'depth', 'item', 'showAll'], 
    name: 'objectTree',
    data() {
      return { showChildren: false }
    },

    computed: {
      ...mapState({
        settings: state => state.kvkSearch.viewSettings,
      }),
      indent() {
        return this.depth > 0 ? { 'padding-left': `25px` } : null
      },
      collapser() {
        return this.isObject(this.value) ? 'collapser' : null;
      },
      formatValue() {
        return this.value ? this.value : this.value == null ? '[null]' : '[empty]';
      },
      iconClasses() {
        return this.showChildren ? ['far', 'minus-square'] : ['far', 'plus-square']
      },
      showEmpty() {
        return this.settings && this.settings.showEmpty
      },
      showChildrenWatch() {
        return this.settings.showChildren
      }
    },

    watch: {
      showChildrenWatch: function () {
        this.showChildren = this.settings.showChildren;
      }
    },

    methods: {
      ...mapActions(['updateKvkSearchViewSettings']),
      isObject(value) {
        return value && typeof value === 'object';
      },
      isArray(value) {
        return this.isObject(value) && value.constructor === Array;
      },
      toggleChildren() {
        this.showChildren = !this.showChildren;
      }
    },

    created() {
      this.showChildren = this.settings.showChildren;
    }
  }
</script>

<style scoped>
  label {
    font-weight: bold;
    margin: 0
  }
    label span {
      font-weight: normal;
    }

  .collapser {
    cursor: pointer;
  }
</style>
