// 树形结构 { id: 111, parentId:  1, children: [] }

/* 数组转化为树  
*/
export const list2tree = function (arr: any[]) {
    arr.forEach(e => {
        arr.forEach(y => {
            if (y.parentId == e.id) {
                if (!e.children) {
                    e.children = []
                }
                e.children.push(y)
            }
        })
    })
    arr = arr.filter(ele => ele.parentId === null)
    return arr
}


/* 树转换为数组 */
export function tree2list(arr: any[]): any[] {
    let res: any[] = []
    let fn = (source: any[]) => {
        source.forEach(el => {
            res.push(el)
            el.children && el.children.length > 0 ? fn(el.children) : ''
        })
    }
    fn(arr)
    return res
}

/* 查找父节点 */
export const getParentNodes = function (nodes: any[], id: string, key = 'id'): any {
    for (let i in nodes) {
        let node = nodes[i]
        if (node[key] == id) {
            return [node]
        }

        if (node.children) {
            let cnode = getParentNodes(node.children, id, key)
            if (cnode !== undefined) {
                return cnode.concat(node)
            }
        }
    }
}

/* 查找父节点 */
export const getParentNode = function (nodes: any[], id: string, key = 'id'): any {
    for (let i in nodes) {
        let node = nodes[i]
        if (node[key] == id) {
            return [node]
        }

        if (node.children) {
            let cnode = getParentNode(node.children, id, key)
            if (cnode) {
                return cnode
            }
        }
    }
}

/* 获取节点 */
export const getNode = function (nodes: any[], id: string): any {
    for (let i in nodes) {
        let item = nodes[i];
        if (item.id === id) {
            return item;
        } else {
            if (item.children) {
                let value = getNode(item.children, id);
                if (value) {
                    return value;
                }
            }
        }
    }
}
